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
            // TODO create settings and new client
            var settings = new ConnectionSettings(new Uri($"http://{config.ElasticHost}:{config.ElasticPort}/{config.ElasticContextRoute}"))
                                .BasicAuthentication(config.ElasticUser, config.ElasticPassword)
                                .DefaultIndex(config.IndexName);
            client = new ElasticClient(settings);
        }

        public void Write(Measurement measurement)
        {
            // TODO upload the measurement into the index
            IIndexResponse response = client.IndexDocument(measurement);

            if (!response.IsValid)
            {
                logger.LogError($"Failed to index document: {response.DebugInformation}");
            }
        }
    }
}