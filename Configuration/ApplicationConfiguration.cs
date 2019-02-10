using Newtonsoft.Json;

namespace adapter.Configuration
{
    public class ApplicationConfiguration
    {
        #region ElasticSearch configuration

        /// <summary>
        /// User used for HTTP basic authentication for the ElasticSearch instance.
        /// </summary>
        /// <value>A username</value>
        [JsonProperty(Required = Required.Always)]
        public string ElasticUser { get; set; } = string.Empty;

        /// <summary>
        /// Password used for HTTP basic authentication for the ElasticSearch instance.
        /// </summary>
        /// <value>A password as string, for example "CorrectHorseBatteryStaple"</value>
        [JsonProperty(Required = Required.Always)]
        public string ElasticPassword { get; set; } = string.Empty;

        /// <summary>
        /// Hostname or IP of the ElasticSearch instance.
        /// </summary>
        /// <value>a hostname like example.org or an IP address</value>
        [JsonProperty(Required = Required.Always)]
        public string ElasticHost { get; set; } = string.Empty;

        /// <summary>
        ///  Route where ElasticSearch is reachable on the target host. For example, 
        ///  in host:port/foo/_cluster/health, "foo" is the context route.
        /// </summary>
        /// <value>A string without '/' before or after</value>
        [JsonProperty(Required = Required.Always)]
        public string ElasticContextRoute { get; set; } = string.Empty;

        /// <summary>
        /// Network port that ElasticSearch is listening on.
        /// </summary>
        /// <value>A port, for example 80 or 443</value>
        [JsonProperty(Required = Required.Always)]
        public uint ElasticPort { get; set; }

        /// <summary>
        /// Name of the ElasticSearch index.
        /// </summary>
        /// <value>string of a valid index name, for example myindex_20190205</value>
        [JsonProperty(Required = Required.Always)]
        public string IndexName { get; set; }

        #endregion

        #region Google Cloud PubSub configuration

        [JsonProperty(Required = Required.Always)]
        public string CredentialsFile { get; set; } = string.Empty;


        [JsonProperty(Required = Required.Always)]
        public string ProjectId { get; set; } = string.Empty;

        [JsonProperty(Required = Required.Always)]
        public string SubscriptionId { get; set; } = string.Empty;

        /// <summary>
        /// Device name to filter messages and retrieve only those from our device.
        /// </summary>
        /// <returns>a device name as string</returns>
        [JsonProperty(Required = Required.Always)]
        public string DeviceName { get; set; } = string.Empty;

        #endregion

        #region Helper methods

        public override string ToString()
        {
            return $"ElasticUser: {ElasticUser}, ElasticPassword: {!string.IsNullOrEmpty(ElasticPassword)}, "
            + $"ElasticHost: {ElasticHost}, ElasticPort: {ElasticPort}, IndexName: {IndexName},"
            + $"ElasticContextRoute: {ElasticContextRoute}, CredentialsFile: {CredentialsFile}"
            + $"SubscriptionId: {SubscriptionId}, DeviceName: {DeviceName}";
        }

        #endregion
    }
}
