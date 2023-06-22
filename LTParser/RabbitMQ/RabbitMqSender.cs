using System.Text;
using RabbitMQ.Client;

namespace LTRuleEngine.RabbitMQ
{
    internal class RabbitMqSender : RabbitMqAbs
    {
        public RabbitMqSender(string exchangeName, string routingKey = "#") : base(exchangeName, routingKey)
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