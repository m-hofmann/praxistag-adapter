using System;
using System.Threading;
using Google.Cloud.PubSub.V1;

namespace adapter
{
    internal class PubSubElasticImporter
    {

        private string projectId = "praxistag-229219";

        private string subscriptionId = "dev-sub";

        public PubSubElasticImporter()
        {

        }

        internal async void Run()
        {
            var subscriptionName = new SubscriptionName(projectId, subscriptionId);
            var subscriber = await SubscriberClient.CreateAsync(subscriptionName);
            await subscriber.StartAsync(
                async (PubsubMessage message, CancellationToken CancellationToken) =>
                {
                    Console.Out.WriteLine(message.Data.ToStringUtf8());
                    return SubscriberClient.Reply.Ack;
                }
            );
        }
    }
}