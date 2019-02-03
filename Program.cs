using System;
using System.IO;
using adapter.Configuration;
using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Logging;
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

            var loggerFactory = new LoggerFactory().AddConsole();
            var mainLogger = loggerFactory.CreateLogger(nameof(Program));

            mainLogger.LogInformation("PubSub <-> ElasticSearch Adapter");
            mainLogger.LogInformation(string.Empty);
            mainLogger.LogInformation($"config.json: {config}");

            var pubSubEsImporter = new PubSubElasticImporter(config, loggerFactory.CreateLogger(nameof(PubSubElasticImporter)));
            var temperatureWriter = new ElasticSearchTemperatureWriter(config, loggerFactory.CreateLogger(nameof(ElasticSearchTemperatureWriter)));
            temperatureWriter.Initialize();
            pubSubEsImporter.Run(m => temperatureWriter.Write(m)).Wait();
        }
    }
}
