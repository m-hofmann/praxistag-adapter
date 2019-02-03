using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using adapter.Configuration;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Grpc.Auth;
using Grpc.Core;
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
            GoogleCredential googleCredential = null;
            using (var jsonStream = new FileStream(Config.CredentialsFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                googleCredential = GoogleCredential.FromStream(jsonStream);
            }
            ChannelCredentials channelCredentials = googleCredential.ToChannelCredentials();

            var subscriptionName = new SubscriptionName(Config.ProjectId, Config.subscriptionId);
            var subscriber = await SubscriberClient.CreateAsync(subscriptionName, new SubscriberClient.ClientCreationSettings(credentials: channelCredentials));

            await subscriber.StartAsync(
                async (PubsubMessage message, CancellationToken CancellationToken) =>
                {
                    try
                    {
                        var measurement = JsonConvert.DeserializeObject<Measurement>(message.Data.ToStringUtf8());
                        Console.WriteLine($"measurement: {measurement}");
                        measurementConsumer(measurement);
                    }
                    catch (Exception e)
                    {
                        // ignore invalid messages and ack them anyway -> avoids busy loop
                        Console.Error.WriteLine($"Failed to parse {e}");
                    }
                    return SubscriberClient.Reply.Ack;
                }
            );
        }
    }
}