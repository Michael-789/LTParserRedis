using LTParser.DTO;
using LTParser.RabbitMQ;
using Newtonsoft.Json;
using System.Text;

namespace LTParser;

public class Parser
{
    RabbitMqSender rabbitMQSender = (RabbitMqSender)RabbitMqFactory.Instance.create(Constants.SENDER, Constants.FLIGHTS_EXCHANGE);

    public void Parse<T>(T obj)
    {
        var jsonObj = JsonConvert.SerializeObject(obj);
       // Console.WriteLine("got " + jsonObj);

        if (obj != null)
        {
            rabbitMQSender.Send(jsonObj);
        }
        

    }
}