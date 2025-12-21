using System.Text;
using System.Text.Json;
using NewsManagementService.Infrastructure.DTOs;
using NewsManagementService.Interfaces.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NewsManagementService.Infrastructure.RabbitMQ.Consumers
{
    public class NotificationsEventsConsumer: BackgroundService, IConsumer
    {
        public string QueueName => "notification.alerts";

        private readonly ILogger<NotificationsEventsConsumer> _logger;
        private readonly IQueueConnection _queueConnection;
        private readonly IServiceScopeFactory _scopeFactory;

        private IChannel _channel;

        public NotificationsEventsConsumer(ILogger<NotificationsEventsConsumer> logger, IQueueConnection queueConnection, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _queueConnection = queueConnection;
            _scopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _channel = await _queueConnection.CreateChannelAsync();

                await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += OnMessageReceived;

                await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer);

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
            var geminiData = JsonSerializer.Deserialize<GeminiRootDto>(message, options);

            if (geminiData != null)
            {
                await newsAppService.SaveNewsSummaryInformation(geminiData);
                _logger.LogInformation("Resumen de noticias de n8n procesado y guardado.");
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
}