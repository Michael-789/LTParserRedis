using LTParser.DTO;
using LTParser.RabbitMQ;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System.Text;

namespace LTParser;

public class Parser
{
    GeoJsonWriter geoJsonWriter = new GeoJsonWriter();
    ReddisHandler reddisHandler = new ReddisHandler();
    //RabbitMqSender rabbitMQSender = (RabbitMqSender)RabbitMqFactory.Instance.create(Constants.SENDER, Constants.FLIGHTS_EXCHANGE);

    public Parser()
    {
        reddisHandler.connect();
    }
    public void Parse<T>(T obj)
    {
        //var jsonObj = JsonConvert.SerializeObject(obj);
        // Console.WriteLine("got " + jsonObj);
        
        var jsonObj = geoJsonWriter.Write(obj);

       

        if (obj != null)
        {
            //rabbitMQSender.Send(jsonObj);
            reddisHandler.push2List("parsed_flights",jsonObj);
        }
        

    }

    
}