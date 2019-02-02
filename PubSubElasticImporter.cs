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

        private JsonSerializer serializer;

        private ApplicationConfiguration Config { get; }

        public PubSubElasticImporter(ApplicationConfiguration config)
        {
            Config = config;

            serializer = new JsonSerializer();
        }

        internal async Task Run(Action<Measurement> measurementConsumer)
        {
            var subscriptionName = new SubscriptionName(Config.ProjectId, Config.subscriptionId);
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