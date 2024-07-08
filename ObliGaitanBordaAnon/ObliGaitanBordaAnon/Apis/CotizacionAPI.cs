namespace ObliGaitanBordaAnon
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class CotizacionAPI
    {
        [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Success { get; set; }

        [JsonProperty("terms", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Terms { get; set; }

        [JsonProperty("privacy", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Privacy { get; set; }

        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public long? Timestamp { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }

        [JsonProperty("quotes", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, double> Quotes { get; set; }
    }
}
