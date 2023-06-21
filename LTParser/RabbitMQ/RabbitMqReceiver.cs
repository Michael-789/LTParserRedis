using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LTParser.RabbitMQ
{
    public class RabbitMqFlightsReceiver : RabbitMqAbs
    {
        private string Queue { get; set; }

        public RabbitMqFlightsReceiver(string exchangeName, string routingKey = "#") : base(exchangeName, routingKey)
        {
            var queueDeclareOk = Channel.QueueDeclare(string.Empty, exclusive: true, autoDelete: true);
            var generatedQueueName = queueDeclareOk.QueueName;
            Channel.QueueBind(generatedQueueName, exchangeName, routingKey);
            Queue = generatedQueueName;
        }

        public void Receive(Action<object> callback)
        {
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (_, ea) =>
            {
                var body = ea.Body.ToArray();
                if (Exchange != ea.Exchange) return;
                var obj = JsonConvert.DeserializeObject<object>(Encoding.UTF8.GetString(body));
                if (obj != null) callback(obj);
            };

            Channel.BasicConsume(queue: Queue,
                autoAck: false,
                consumer: consumer);
        }
    }
}