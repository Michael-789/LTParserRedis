using System.Text;
using RabbitMQ.Client;

namespace LTParser.RabbitMQ
{
    internal class RabbitMqAlertsSender : RabbitMqAbs
    {
        public RabbitMqAlertsSender(string exchangeName, string routingKey = "#") : base(exchangeName, routingKey)
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