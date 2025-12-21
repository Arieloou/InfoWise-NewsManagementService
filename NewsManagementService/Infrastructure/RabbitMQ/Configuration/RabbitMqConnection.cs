using Microsoft.Extensions.Options;
using NewsManagementService.Interfaces.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace NewsManagementService.Infrastructure.RabbitMQ.Configuration
{
    
public class RabbitMqConnection : IQueueConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private static readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1); 
        private readonly ILogger<RabbitMqConnection> _logger;

        private IConnection? _connection;

        public RabbitMqConnection(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqConnection> logger)
        {
            _logger = logger;
            var config = options.Value;

            _connectionFactory = new ConnectionFactory()
            {
                HostName = config.HostName,
                Port = config.Port,
                VirtualHost = config.VirtualHost,
                UserName = config.Username,
                Password = config.Password 
            };
        }

        public bool IsConnected => _connection != null && _connection.IsOpen;

        private async Task TryConnectAsync(CancellationToken cancellationToken = default)
        {
            if (IsConnected) return;

            _logger.LogInformation("Attempting to establish a connection with RabbitMQ...");

            await _connectionLock.WaitAsync(cancellationToken);
            try
            {
                if (IsConnected) return;

                _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

                _logger.LogInformation("Connection to RabbitMQ successfully established.");
            }
            catch (BrokerUnreachableException ex)
            {
                _logger.LogCritical(ex, "Could not connect to RabbitMQ. The host is unreachable.");
                throw;
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        public async Task<IChannel> CreateChannelAsync()
        {
            if (!IsConnected)
            {
                await TryConnectAsync();
            }

            return await _connection!.CreateChannelAsync();
        }

        public void Dispose()
        {
            try
            {
                _connection?.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error closing the RabbitMQ connection.");
            }
        }
    }   
}