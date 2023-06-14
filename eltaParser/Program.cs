using eltaParser;
using EltaParser;

Console.WriteLine("Hello,please enter 1 to start");
int start = Convert.ToInt32(Console.ReadLine());
if (start == 1)
{
    Console.WriteLine("Press X to stop");

    RabbitMqHandler rabbitMqHandler = new RabbitMqHandler();
    string[] queues = { "alerts" };
    string[] exchanges = { "alerts" };
    rabbitMqHandler.Receive(queues, exchanges, "#");
    DataCreator.Create();
}