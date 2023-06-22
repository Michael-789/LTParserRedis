using LTParser;
using LTParser.DTO;
using LTRuleEngine.RabbitMQ;

Console.WriteLine("Press X to stop");
var parser = new Parser();
new RabbitMqReceiver("flights").Receive<Flight>(parser.Parse);
Console.ReadLine();