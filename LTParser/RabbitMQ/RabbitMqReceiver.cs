using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LTParser.RabbitMQ
{
    public class RabbitMqReceiver : RabbitMqAbs
    {
        private static RabbitMqReceiver instance;

        public static RabbitMqReceiver getInstance(string exchangeName, string routingKey = "#")
        {

            if (instance == null)
            {
                instance = new RabbitMqReceiver(exchangeName, routingKey);
            }
            return instance;

        }
        private string Queue { get; set; }

        private RabbitMqReceiver(string exchangeName, string routingKey = "#") : base(exchangeName, routingKey)
        {
            var queueDeclareOk = Channel.QueueDeclare(string.Empty, exclusive: true, autoDelete: true);
            var generatedQueueName = queueDeclareOk.QueueName;
            Channel.QueueBind(generatedQueueName, exchangeName, routingKey);
            Queue = generatedQueueName;
        }

        public void Receive<T>(Action<T> callback)
        {
            var consumer = new EventingBasicConsumer(Channel);
            Console.WriteLine(Assembly.GetEntryAssembly().GetName().Name +
                              " start and waiting for messages from exchange " + Exchange);
            consumer.Received += (_, ea) =>
            {
                var body = ea.Body.ToArray();
                if (Exchange != ea.Exchange) return;
                var obj = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                if (obj != null) callback(obj);
            };

            Channel.BasicConsume(queue: Queue,
                autoAck: false,
                consumer: consumer);
        }
    }
}