using System.Text;
using System.Text.Json;
using NewsManagementService.Application;
using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Interfaces.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NewsManagementService.Infrastructure.RabbitMQ.Consumers;

public class UserPreferencesConsumer : BackgroundService, IConsumer
{
    public string QueueName { get; } = "user_preferences_to_sync";
    
    private readonly ILogger<N8NEventsConsumer> _logger;
    private readonly IQueueConnection _queueConnection;
    private readonly IServiceScopeFactory _scopeFactory;

    public UserPreferencesConsumer(ILogger<N8NEventsConsumer> logger, IQueueConnection queueConnection, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _queueConnection = queueConnection;
        _scopeFactory = scopeFactory;
    }
    
    private IChannel _channel;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _channel = await _queueConnection.CreateChannelAsync();

            await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += OnMessageReceived;

            await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

            _logger.LogInformation("Consumer started. Waiting for messages in queue '{QueueName}'.", QueueName);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");
        }
    }
    
    private async Task OnMessageReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        using var scope = _scopeFactory.CreateScope();

        try
        {
            await ProcessEvent(message, scope);
            await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
        }
        catch (Exception ex)
        {
            await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: false);
            _logger.LogError(ex, "Error processing message: {Message}", message);
        }
    }

    private async Task ProcessEvent(string message, IServiceScope scope)
    {
        var newsAppService = scope.ServiceProvider.GetRequiredService<NewsAppService>();
            
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var userPreferencesData = JsonSerializer.Deserialize<UserPreferencesDto>(message, options);

        if (userPreferencesData != null)
        {
            await newsAppService.SaveUserPreferencesInformation(userPreferencesData);
            _logger.LogInformation("User preferences processed and saved.");
        }
            
        await Task.Delay(100);
    }

    public override void Dispose()
    {
        _channel?.CloseAsync();
        _channel?.Dispose();
        base.Dispose();
    }
}