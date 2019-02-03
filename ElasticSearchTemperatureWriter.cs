using System;
using adapter.Configuration;
using Microsoft.Extensions.Logging;
using Nest;

namespace adapter
{
    internal class ElasticSearchTemperatureWriter
    {
        private readonly ApplicationConfiguration config;
        
        private readonly ILogger logger;

        private ElasticClient client;

        public ElasticSearchTemperatureWriter(ApplicationConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public void Initialize()
        {
            var settings = new ConnectionSettings(new Uri($"http://{config.ElasticHost}:{config.ElasticPort}/{config.ElasticContextRoute}"))
                                .BasicAuthentication(config.ElasticUser, config.ElasticPassword)
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
                logger.LogError($"Failed to index document: {response.DebugInformation}");
            }
        }
    }
}