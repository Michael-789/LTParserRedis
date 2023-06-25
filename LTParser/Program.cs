using LTParser;
using LTParser.DTO;
using LTParser.RabbitMQ;

Console.WriteLine("Press X to stop");
var parser = new Parser();
RabbitMqReceiver rabbitMQReceiver = (RabbitMqReceiver)RabbitMqFactory.Instance.create(Constants.RECIEVER, Constants.RAW_FLIGHTS_EXCHANGE);

rabbitMQReceiver.Receive<Flight>(parser.Parse);
Console.ReadLine();