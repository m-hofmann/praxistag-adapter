using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using adapter.Configuration;
using Google.Cloud.PubSub.V1;
using Newtonsoft.Json;

namespace adapter
{
    internal class PubSubElasticImporter
    {

        // FIXME from application config
        private string projectId = "praxistag-229219";

        // FIXME from application config
        private string subscriptionId = "dev-sub";

        private JsonSerializer serializer;

        private ApplicationConfiguration Config { get; set; }

        public PubSubElasticImporter(ApplicationConfiguration config)
        {
            Config = config;

            serializer = new JsonSerializer();
        }

        internal async Task Run(Action<Measurement> measurementConsumer)
        {
            var subscriptionName = new SubscriptionName(projectId, subscriptionId);
            var subscriber = await SubscriberClient.CreateAsync(subscriptionName);

            await subscriber.StartAsync(
                async (PubsubMessage message, CancellationToken CancellationToken) =>
                {
                    var measurement = JsonConvert.DeserializeObject<Measurement>(message.Data.ToString());
                    Console.WriteLine($"measurement: {measurement}");
                    measurementConsumer(measurement);
                    return SubscriberClient.Reply.Ack;
                }
            );
        }
    }
}