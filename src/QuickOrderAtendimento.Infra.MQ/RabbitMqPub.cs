﻿using Microsoft.Extensions.Options;
using QuickOrderAtendimento.Application.Events;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace QuickOrderAtendimento.Infra.MQ
{
    [ExcludeFromCodeCoverage]
    public class RabbitMqPub<T> : IRabbitMqPub<T> where T : class
    {
        private readonly IModel _channel;

        public RabbitMqPub(IOptions<RabbitMqSettings> configuration)
        {
            var factory = new ConnectionFactory
            {
                // "guest"/"guest" by default, limited to localhost connections
                UserName = configuration.Value.UserName,
                Password = configuration.Value.Password,
                VirtualHost = "/",
                HostName = configuration.Value.Host,
                Port = Int32.Parse(configuration.Value.Port)
            };

            IConnection connection = factory.CreateConnection();

            _channel = connection.CreateModel();
        }

        public void Publicar(T obj, string routingKey, string queue)
        {
            _channel.QueueDeclare(queue: queue,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            string mensagem = JsonSerializer.Serialize(obj);
            var body = Encoding.UTF8.GetBytes(mensagem);

            _channel.BasicPublish(exchange: "Atendimento",
                routingKey: routingKey,
                basicProperties: null,
                body: body
                );
        }

    }
}
