using LTParser;
using LTParser.DTO;
using LTParser.RabbitMQ;


    Console.WriteLine("Press X to stop");

    RabbitMqHandler rabbitMqHandler = new RabbitMqHandler();
    string[] queues = { "alerts" };
    string[] exchanges = { "alerts" };
    rabbitMqHandler.Receive(queues, exchanges, "#");
    DataCreator.Create();


new RabbitMqReceiver("flights").Receive<Flight>(.Send);
Console.ReadLine();