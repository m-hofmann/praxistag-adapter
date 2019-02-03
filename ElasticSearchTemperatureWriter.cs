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
                                .BasicAuthentication(config.ElasticUser, config.ElasticPassword)
                                .EnableTcpKeepAlive(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
                                .DefaultIndex("dev");
            client = new ElasticClient(settings);
        }

        public void Write(Measurement measurement)
        {
            var retryCount = 0;
            IIndexResponse response;
            do 
            {
                response = client.IndexDocument(measurement);
                retryCount++;
            } while (!response.IsValid && retryCount <= 3);

            if (!response.IsValid)
            {
                Console.Error.WriteLine($"Failed to index document: {response.DebugInformation}");
            }
        }
    }
}