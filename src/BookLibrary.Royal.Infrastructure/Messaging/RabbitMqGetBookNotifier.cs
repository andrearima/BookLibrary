using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BookLibrary.Royal.Infrastructure.Messaging;

public class RabbitMqGetBookNotifier : IRabbitMqGetBookNotifier, IDisposable
{
    const string QueueName = "BookSearch";
    private readonly RabbitMqConfiguration _rabbitMqConfiguration;
    private readonly IConnection _connection;
    private readonly IModel _model;
    private readonly ILogger<RabbitMqGetBookNotifier> _logger;
    private bool disposedValue;

    public RabbitMqGetBookNotifier(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqGetBookNotifier> logger)
    {
        _rabbitMqConfiguration = options.Value;
        _connection = CreateChannel();
        _model = _connection.CreateModel();
        _model.QueueDeclare(queue: QueueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        _model.ExchangeDeclare(QueueName, ExchangeType.Fanout, durable: true, autoDelete: false);
        _model.QueueBind(QueueName, QueueName, string.Empty);
        _logger = logger;
    }

    private IConnection CreateChannel()
    {
        ConnectionFactory connection = new ConnectionFactory()
        {
            UserName = _rabbitMqConfiguration.Username,
            Password = _rabbitMqConfiguration.Password,
            HostName = _rabbitMqConfiguration.HostName,
            Port = _rabbitMqConfiguration.Port
        };
        connection.DispatchConsumersAsync = true;

        return connection.CreateConnection();
    }

    public void SendMessage<T>(T message)
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            _model.BasicPublish(QueueName,
                                string.Empty,
                                basicProperties: null,
                                body: body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on publish event on RabbitMq");
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _connection?.Dispose();
                _model?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
