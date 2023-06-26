using LTParser.DTO;
using LTParser.RabbitMQ;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System.Text;

namespace LTParser;

public class Parser
{
    GeoJsonWriter geoJsonWriter = new GeoJsonWriter();
    RabbitMqSender rabbitMQSender = (RabbitMqSender)RabbitMqFactory.Instance.create(Constants.SENDER, Constants.FLIGHTS_EXCHANGE);

    public void Parse<T>(T obj)
    {
        //var jsonObj = JsonConvert.SerializeObject(obj);
        // Console.WriteLine("got " + jsonObj);
        
        var jsonObj = geoJsonWriter.Write(obj);

        if (obj != null)
        {
            rabbitMQSender.Send(jsonObj);
        }
        

    }
}