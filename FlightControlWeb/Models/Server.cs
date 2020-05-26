using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Models
{
    public class Server
    {
/*        [IgnoreDataMember]
        public string id { get; set; }*/

        [JsonPropertyName("server_id")]
        public string Server_ID { get; set; }

        [JsonPropertyName("server_url")]
        public string Server_URL { get; set; }
    }
}
