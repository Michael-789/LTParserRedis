using LTParser;
using LTParser.DTO;
using LTParser.RabbitMQ;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Nodes;

var parser = new Parser();
RabbitMqReceiver rabbitMQReceiver = (RabbitMqReceiver)RabbitMqFactory.Instance.create(Constants.RECIEVER, Constants.RAW_FLIGHTS_EXCHANGE);

rabbitMQReceiver.Receive<Flight>(parser.Parse);
Console.ReadLine();