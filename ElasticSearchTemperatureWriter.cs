using System;
using adapter.Configuration;
using Nest;

namespace adapter
{
    internal class ElasticSearchTemperatureWriter
    {
        private ApplicationConfiguration config;

        private ElasticClient client;

        public ElasticSearchTemperatureWriter(ApplicationConfiguration config)
        {
            this.config = config;
        }

        public void Initialize()
        {
            var settings = new ConnectionSettings(new Uri($"http://{config.ElasticHost}:{config.ElasticPort}/{config.ElasticContextRoute}"))
                                .DefaultIndex("dev");
            var client = new ElasticClient(settings);
        }

        public void Write(Measurement measurement)
        {
            var indexResponse = client.IndexDocument(measurement);
            if (!indexResponse.IsValid)
            {
                Console.Error.WriteLine($"Failed to index document: {indexResponse}");
            }
        }
    }
}