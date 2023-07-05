using NetTopologySuite.IO;
using Newtonsoft.Json;
using StackExchange.Redis;
using NetTopologySuite.IO;
using System.Text;

namespace LTParser
{
    internal class ReddisHandler
    {
        ConnectionMultiplexer redis;
        IDatabase db;
        GeoJsonReader geoJsonReader;

        public ReddisHandler() { }

        public void connect()
        {
            redis = ConnectionMultiplexer.Connect("localhost");
            db = redis.GetDatabase();
            geoJsonReader = new GeoJsonReader();
        }

        public void push2List(string listName, string json)
        {
            db.ListRightPush(listName, json);
        }

        public void popFromList<T>(Action<T> callback,string listName) where T : class
        {
            int secCounter = 0;
            while (secCounter < 10)
            {
                RedisValue popObj = db.ListLeftPop(listName);
                if (popObj.HasValue)
                {
                    //var json = result.ToString();
                    var obj = geoJsonReader.Read<T>(popObj.ToString());
                    //Console.WriteLine(str);
                    // var obj = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                    if (obj != null) callback(obj);
                    //var jsonObject = JsonConvert.DeserializeObject(jsonObj);
                    Console.WriteLine($"Popped JSON object: {popObj.ToString()}");
                    secCounter = 0;
                }
                else
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("No JSON object available. Waiting for 1 seconds...");
                    secCounter++;
                }
            }
        }


    }
}
