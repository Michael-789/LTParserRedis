using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace EltaParser
{
    internal class RabbitMQHandler
    {
        ConnectionFactory factory;
        IConnection connection;
        IModel channel;
        int counter = 0;
       public RabbitMQHandler() 
        {
            factory = new ConnectionFactory { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }
        public void Send(string exchange, string routingKey, object data)
        {

            channel.ExchangeDeclare(exchange, type: ExchangeType.Topic);



            byte[] body = Encoding.Default.GetBytes(data.ToString());



            channel.BasicPublish(exchange,
                     routingKey,
                     basicProperties: null,
                     body: body);
            Console.WriteLine($" [x] Sent {data}");
            
        }

        public void SendBinary(string queue, object data)
        {


            channel.QueueDeclare(queue: queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);



            byte[] body = SerialiseIntoBinary(data);

            channel.BasicPublish(string.Empty,
                                  queue,
                                 null,
                                  body);
            Console.WriteLine($" [x] Sent {data}");
        
        }

        public  void Receive(string exchange, string routingKey)
        {

            counter = 0;

            channel.ExchangeDeclare(exchange, type: ExchangeType.Topic);

            var queueName = channel.QueueDeclare().QueueName;



            channel.QueueBind(queueName,exchange,routingKey);



            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var data = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine($" [x] Received '{routingKey}':'{data}'");
                Flight flight = JsonConvert.DeserializeObject<Flight>(data);
                asyncSend("parsed_objects_exchange", "parsed.data.info", flight);
                Console.WriteLine("recieved {0} ",++counter);

            };

            channel.BasicConsume(queue: queueName,
                     autoAck: false,
                     consumer: consumer);

  
 
        }

        public  void ReceiveBinary(string queue)
        {
          
            channel.QueueDeclare(queue: queue,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
        
            BasicGetResult result = channel.BasicGet(queue, true);
            while (result != null)
            {
                Flight flight = (Flight)DeserialiseFromBinary(result.Body.ToArray());    

                result = channel.BasicGet(queue, true);
            }
       
        }

        private  void OnNewMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine($"Message: {Encoding.UTF8.GetString(e.Body.ToArray())}");
            Console.WriteLine("Press any key to stop consuming message.");
        }
        private static byte[] SerialiseIntoBinary(object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, obj);
            memoryStream.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.GetBuffer();
        }

        private  object DeserialiseFromBinary(byte[] messageBody)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(messageBody, 0, messageBody.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return binaryFormatter.Deserialize(memoryStream) ;
        }


        async void asyncSend(string exchange, string routingKey, object data)
        {
            await Task.Run(() => Send( exchange,  routingKey,  data));
        }

    }
}
