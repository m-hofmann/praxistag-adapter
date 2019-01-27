using System;
using System.IO;
using adapter.Configuration;
using Google.Cloud.PubSub.V1;
using Newtonsoft.Json;

namespace adapter
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationConfiguration config;

            using (StreamReader reader = File.OpenText("config.json"))
            {
                var serializer = new JsonSerializer();
                config = (ApplicationConfiguration) serializer.Deserialize(reader, typeof(ApplicationConfiguration));
            }

            Console.WriteLine("PubSub <-> ElasticSearch Adapter");
            Console.WriteLine(string.Empty);
            Console.WriteLine($"config.json: {config}");

            var pubSubEsImporter = new PubSubElasticImporter();
            pubSubEsImporter.Run();
        }
    }
}
