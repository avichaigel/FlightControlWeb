﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
	public class Segment
	{
/*		[IgnoreDataMember]
		public string id { get; set; }*/

		[JsonPropertyName("longitude")]
		public double Longitude { get; set; }

		[JsonPropertyName("latitude")]
		public double Latitude { get; set; }

		[JsonPropertyName("timespan_seconds")]
		public int Timespan_Seconds { get; set; }
	}
}
