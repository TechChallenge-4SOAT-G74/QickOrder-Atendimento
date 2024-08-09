using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QuickOrderAtendimento.Infra.MQ
{
    [ExcludeFromCodeCoverage]
    public class RabbitMqSub : BackgroundService
    {
        private readonly string _nomeDaFila;
        private IModel _channel;
        private IProcessaEvento _processaEvento;
        private readonly string _exchange = "QuickOrder";

        public RabbitMqSub(IOptions<RabbitMqSettings> configuration, IProcessaEvento processaEvento)
        {
            var factory = new ConnectionFactory
            {
                UserName = configuration.Value.UserName,
                Password = configuration.Value.Password,
                VirtualHost = "/",
                HostName = configuration.Value.Host,
                Port = Int32.Parse(configuration.Value.Port)
            };

            IConnection connection = factory.CreateConnection();

            _channel = connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);
            _nomeDaFila = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _nomeDaFila, exchange: _exchange, routingKey: "Atendimento");
            _processaEvento = processaEvento;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            EventingBasicConsumer? consumidor = new EventingBasicConsumer(_channel);

            consumidor.Received += (ModuleHandle, ea) =>
            {
                ReadOnlyMemory<byte> body = ea.Body;
                string? mensagem = Encoding.UTF8.GetString(body.ToArray());
                _processaEvento.Processa(mensagem);
            };

            _channel.BasicConsume(queue: _nomeDaFila, autoAck: true, consumer: consumidor);

            return Task.CompletedTask;
        }
    }
}
