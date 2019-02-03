using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace adapter
{
    public class Measurement
    {
        /// <summary>
        /// A random id for use in ElasticSearch. This property is initialized
        /// with a new random GUID upon creation of a Measurement object.
        /// </summary>
        /// <returns>A randomly initialized GUID</returns>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// ID of the device that sent the measurement
        /// </summary>
        /// <value>A device name as string</value>
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        /// <summary>
        /// Measurement timestamp
        /// </summary>
        /// <value>Datetime object</value>
        [JsonProperty("time_stamp")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        /// <summary>
        /// Temperature measurement in degree celsius
        /// </summary>
        /// <value>temperature in °C</value>
        [JsonProperty("temperature")]
        public Double TemperatureCelsius { get; set; }

        /// <summary>
        /// Humidity measurement in percent.
        /// </summary>
        /// <value>A relative humidity like 0.42</value>
        [JsonProperty("humidity")]
        public Double Humidity { get; set; }

        override public string ToString() 
        {
            return $"Device: {DeviceId}, timestamp: {TimeStamp}, temperature: {TemperatureCelsius}, humidity: {Humidity}";
        }
    }
}
