using System.Text;
using RabbitMQ.Client;

namespace LTParser.RabbitMQ
{
    internal class RabbitMqSender : RabbitMqAbs
    {
        private static RabbitMqSender instance;

        public static RabbitMqSender getInstance(string exchangeName, string routingKey = "#")
        {

            if (instance == null)
            {
                instance = new RabbitMqSender(exchangeName, routingKey);
            }
            return instance;

        }
        private RabbitMqSender(string exchangeName, string routingKey = "#") : base(exchangeName, routingKey)
        {
        }

        public void Send(string msg)
        {
            byte[] byteAlert = Encoding.Default.GetBytes(msg);

            Channel.BasicPublish(Exchange,
                RoutingKey,
                basicProperties: null,
                body: byteAlert);
        }
    }
}