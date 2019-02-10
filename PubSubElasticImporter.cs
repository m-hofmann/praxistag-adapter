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
                    if (IsMessageFromMyDevice(message))
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
                    }
                    // ack all messages so they do not occur again 
                    return SubscriberClient.Reply.Ack;
                }
            );
        }

        /// <summary>
        /// Initialize a new pubsub subscriber client with the provided credentials and
        /// the subscription id read from the global app configuration.
        /// </summary>
        /// <returns>an initialized subscriberclient</returns>
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

        /// <summary>
        /// Determine whether a message is from the device configured for this application instance.
        /// </summary>
        /// <param name="message"> a pubsub message</param>
        /// <returns>true if this device is to be handled by this application</returns>
        private bool IsMessageFromMyDevice(PubsubMessage message) 
        {
            var messageDeviceName = message.Attributes["deviceId"] as string;
            return config.DeviceName.Equals(messageDeviceName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}