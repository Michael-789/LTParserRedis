
using EltaParser;



RabbitMQHandler rh = new RabbitMQHandler();

rh.Receive("topic_flights", "flight.info");
Console.ReadLine();

