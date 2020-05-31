using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using FlightControlWeb.Controllers;
    
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

        bool isExternal = false;
        [JsonPropertyName("is_external")]
        bool Is_External { get; set; }

        public Flights(KeyValuePair<string, FlightPlan> plan, bool isExternal1, string relativeTo,
            DateTime initialDT, DateTime relativeToDT)
        {
            Flight_ID = plan.Key;
            calcLongLat(plan, initialDT, relativeToDT); //determine Longitude and Latitude
            Passengers = plan.Value.Passengers;
            Company_Name = plan.Value.Company_Name;
            Date_Time = relativeTo;
            Is_External = isExternal1;
        }

        private void calcLongLat(KeyValuePair<string, FlightPlan> plan, DateTime initialDT,
            DateTime relativeToDT)
        {
            int cumulativeTimespan=0, segIndex=0;
            Segment currSeg= null, lastSeg= null;
            DateTime segStartDT = initialDT, segEndDT;
            //calculate percentage of time flown in current segment
            //first determine in which segment we are according to relativeTo
            foreach (Segment seg in plan.Value.Segments)
            {
                segIndex++;
                cumulativeTimespan += seg.Timespan_Seconds;
                segEndDT = initialDT.AddSeconds(cumulativeTimespan);
                if ((DateTime.Compare(segEndDT, relativeToDT) >= 0))
                {
                    segStartDT = segEndDT.AddSeconds(-seg.Timespan_Seconds);
                    currSeg = seg;
                    break;
                }
            }
            //create lastSeg
            if (segIndex > 0)
            {
                lastSeg = plan.Value.Segments[segIndex-1];
            } else if (segIndex == 0)
            {
                lastSeg = currSeg;
            }
            //calculate the timespan between the end of the last segment and the relativeToDT
            TimeSpan progressInSegment = relativeToDT - segStartDT;
            double timeProgPercent = progressInSegment.TotalSeconds / currSeg.Timespan_Seconds;
            //calculate distance in segment
            double distance = Math.Sqrt(Math.Pow(currSeg.Latitude - lastSeg.Latitude, 2) +
               Math.Pow(currSeg.Longitude - lastSeg.Longitude, 2));
            //calculate distance relatively to time
            double distRelTime = timeProgPercent * distance;
            //determine alpha
            double alpha = Math.Cos(Math.Abs(currSeg.Latitude - lastSeg.Latitude) / distance) *
                180.0 / Math.PI;
            Latitude = lastSeg.Latitude + distRelTime * Math.Sin(alpha);
            Longitude = lastSeg.Longitude + distRelTime * Math.Cos(alpha);
        }
    }
}
