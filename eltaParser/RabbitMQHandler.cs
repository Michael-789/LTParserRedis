using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EltaParser
{
    internal class RabbitMqHandler
    {
        private readonly IModel _channel;
        private int _counter;

        public RabbitMqHandler()
        {
            var factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public void Send(string queue, string exchange, string routingKey, byte[] body)
        {
            _channel.ExchangeDeclare(exchange, type: ExchangeType.Topic);
            _channel.QueueDeclare(queue, exclusive: false);

            // Console.WriteLine(Encoding.UTF8.GetString(body));
            _channel.BasicPublish(exchange,
                routingKey,
                basicProperties: null,
                body: body);
        }

        public void Receive(string[] queues, string[] exchanges, string routingKey)
        {
            _counter = 0;

            List<string> queueNamesList = new List<string>();

            for (int i = 0; i < queues.Length; i++)
            {
                _channel.ExchangeDeclare(exchanges[i], type: ExchangeType.Topic);

                var queueName = _channel.QueueDeclare(queues[i]);

                queueNamesList.Add(queueName);

                _channel.QueueBind(queueName, exchanges[i], routingKey);
            }
            
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var data = Encoding.UTF8.GetString(body);
                if ("alerts" == ea.Exchange)
                {
                    Console.WriteLine("ALERT!! '{0}", data);
                    Console.WriteLine("received {0} alerts ", ++_counter);
                }
            };

            foreach (string queueName in queueNamesList)
            {
                _channel.BasicConsume(queue: queueName,
                    autoAck: false,
                    consumer: consumer);
            }
        }
    }
}