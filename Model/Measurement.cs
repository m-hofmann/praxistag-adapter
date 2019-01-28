using System;
using Newtonsoft.Json;

namespace adapter
{
    public class Measurement
    {
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
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Temperature measurement in degree celsius
        /// </summary>
        /// <value>temperature in °C</value>
        [JsonProperty("temperature_celsius")]
        public Double TemperatureCelsius { get; set; }
    }
}
