using System.Text;
using System.Text.Json.Nodes;
using EltaParser;

namespace eltaParser
{
    internal static class DataCreator
    {
        private const int X = 750;
        private const int Y = 500;
        private const int Z = 1000;

        private const int Speed = 850;

        private const int MsgAmount = 600;

        private static int _id = 1;

        public static void Create()
        {
            RabbitMqHandler rabbitMqHandler = new RabbitMqHandler();


            int secondsCounter = 0;

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.X))
            {
                List<byte[]> arrayList = CreateData();
                DateTime startTime = DateTime.Now;

                // Console.WriteLine("arrayList " + arrayList.Count);
                int listCount = 0;
                foreach (byte[] body in arrayList)
                {
                    listCount++;
                    //byte[] body = arrayList[i];
                    rabbitMqHandler.Send("flights", "#", body);
                }

                DateTime endTime = DateTime.Now;

                TimeSpan ts = (endTime - startTime);


                //Console.WriteLine("Elapsed Time is {0} ms", ts.TotalMilliseconds);

                if (ts.TotalMilliseconds < 1000)
                {
                    int ms = (int)Math.Round((1000 - ts.TotalMilliseconds), 0);
                    //Console.WriteLine("going to sleep for {0} milliseconds", ms);
                    Thread.Sleep(ms);
                }

                secondsCounter++;
                // if ((secondsCounter % 1) == 0)
                //{
                Console.WriteLine("Processed {0} during {1} seconds", listCount * secondsCounter, secondsCounter);
                // }
            }
        }

        private static List<byte[]> CreateData()
        {
            List<byte[]> arrayList = new List<byte[]>();

            int newX = X, newY = Y, newZ = Z;

            for (int i = 0; i < MsgAmount; i++)
            {
                JsonObject flightJson = new JsonObject();
                if ((_id % 10) == 0)
                {
                    newZ = 1001;
                }
                else if ((_id % 5) == 0)
                {
                    newX = 42;
                    newY = 42;
                }

                flightJson.Add("Id", _id++);

                flightJson.Add("location", new JsonObject()
                {
                    { "longitude", newX },
                    { "latitude", newY },
                    { "altitude", newZ }
                });

                flightJson.Add("speed", Speed);
                //Console.WriteLine(flightJson.ToString());

                byte[] body = Encoding.Default.GetBytes(flightJson.ToString());
                arrayList.Add(body);
            }

            return arrayList;
        }
    }
}