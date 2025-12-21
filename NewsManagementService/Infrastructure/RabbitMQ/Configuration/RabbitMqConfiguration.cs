namespace NewsManagementService.Infrastructure.RabbitMQ.Configuration;

public class RabbitMqConfiguration
{
    public required string HostName { get; set; }

    public int Port { get; set; }

    public required string VirtualHost { get; set; }

    public required string Username { get; set; }

    public required string Password { get; set; }
}