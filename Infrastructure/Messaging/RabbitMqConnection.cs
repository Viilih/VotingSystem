using RabbitMQ.Client;

namespace Infrastructure.Messaging;

public class RabbitMqConnection : IDisposable
{
    private readonly IConnectionFactory _connectionFactory;
    private IConnection _connection;
    private bool _disposed;

    public RabbitMqConnection(string hostName, string userName, string password)
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 4672,
            UserName = "guest",
            Password = "guest",
            DispatchConsumersAsync = true // Supports async consumers
        };
    }
    
    public IConnection GetConnection()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            _connection = _connectionFactory.CreateConnection();
        }

        return _connection;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        _connection?.Dispose();
    }
}