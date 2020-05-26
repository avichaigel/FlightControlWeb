using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Models

{
    public class Flights
    {
/*        [IgnoreDataMember]
        public string id { get; set; }*/
        [JsonPropertyName("flight_id")]
        public string Flight_ID { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("passengers")]
        public int Passengers { get; set; }

        [JsonPropertyName("company_name")]
        public string Company_Name { get; set; }

        [JsonPropertyName("date_time")]
        public string Date_Time { get; set; }

        bool is_external = false;
        [JsonPropertyName("is_external")]
        bool Is_External { get; set; }
    }
}
