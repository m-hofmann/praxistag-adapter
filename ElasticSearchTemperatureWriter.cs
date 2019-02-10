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
            
        }

        public void Write(Measurement measurement)
        {
            // TODO upload the measurement into the index
            
            logger.LogInformation($"Should upload {measurement} but doing nothing");
        }
    }
}