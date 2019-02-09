using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using adapter.Configuration;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Grpc.Auth;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace adapter
{
    internal class PubSubClient
    {

        private readonly JsonSerializer serializer;

        private readonly ILogger logger;

        private readonly ApplicationConfiguration config;

        public PubSubClient(ApplicationConfiguration config, Microsoft.Extensions.Logging.ILogger logger)
        {
            this.config = config;
            this.logger = logger;

            this.serializer = new JsonSerializer();
        }

        internal async Task Run(Action<Measurement> measurementConsumer)
        {
            var subscriber = await InitializeClient();
           
            await subscriber.StartAsync(
                async (PubsubMessage message, CancellationToken CancellationToken) =>
                {
                    try
                    {
                        var measurement = JsonConvert.DeserializeObject<Measurement>(message.Data.ToStringUtf8());
                        logger.LogInformation($"measurement: {measurement}");
                        measurementConsumer(measurement);
                    }
                    catch (Exception e)
                    {
                        // ignore invalid messages and ack them anyway -> avoids busy loop
                        logger.LogError("Failed to parse event", e);
                    }
                    return SubscriberClient.Reply.Ack;
                }
            );
        }

        private async Task<SubscriberClient> InitializeClient() {
            GoogleCredential googleCredential = null;
            using (var jsonStream = new FileStream(config.CredentialsFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                googleCredential = GoogleCredential.FromStream(jsonStream);
            }
            ChannelCredentials channelCredentials = googleCredential.ToChannelCredentials();

            var subscriptionName = new SubscriptionName(config.ProjectId, config.SubscriptionId);
            return await SubscriberClient.CreateAsync(subscriptionName, new SubscriberClient.ClientCreationSettings(credentials: channelCredentials));
        }
    }
}