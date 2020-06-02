using FlightControlWeb.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
	public class FlightsManager
	{
		public FlightsManager() { }

		private List<double> calcLongLat(KeyValuePair<string, FlightPlan> plan, DateTime initialDT,
			DateTime relativeToDT)
		{
			int cumulativeTimespan = 0, segIndex = 0;
			Segment currSeg = new Segment(), lastSeg = new Segment();
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
				lastSeg = plan.Value.Segments[segIndex - 1];
			}
			else if (segIndex == 0)
			{
				lastSeg = currSeg;
			}
			//calculate the timespan between the end of the last segment and relativeToDT
			TimeSpan progress = relativeToDT - segStartDT;
			/*double timeProgPercent = progressInSegment.TotalSeconds / currSeg.Timespan_Seconds;
			//calculate distance in segment
			double distance = Math.Sqrt(Math.Pow(currSeg.Latitude - lastSeg.Latitude, 2) +
			   Math.Pow(currSeg.Longitude - lastSeg.Longitude, 2));
			//calculate distance relatively to time
			double distRelTime = timeProgPercent * distance;
			//determine alpha
			double alpha = Math.Cos(Math.Abs(currSeg.Latitude - lastSeg.Latitude) / distance) *
				180.0 / Math.PI;*/
			return new List<double>() {
				lastSeg.Latitude + progress.TotalSeconds*(currSeg.Latitude-lastSeg.Latitude),
				lastSeg.Longitude + progress.TotalSeconds*(currSeg.Longitude-lastSeg.Longitude),
			};
		}

		public List<Flights> GetActiveInternals(string relativeTo, bool isExternal)
		{
			List<Flights> activeFlights = new List<Flights>();
			//create a DateTime object out of the relatieTo argument
			DateTime relativeToDT = TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(relativeTo));

			//iterate through the flightPlans in our server, and if relativeTo's DateTime is 
			//between the plan's initial and final DateTime, add it to activeFlights array
			foreach (KeyValuePair<string, FlightPlan> plan in FlightPlanController.plansDict)
			{
				int totalFlightTime = 0;
				//create a DateTime object out of the flight plan's Date_Time string
				DateTime planInitialDT = TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(
						plan.Value.Initial_Location.Date_Time));
				//calculate total flight time in seconds
				foreach (Segment seg in plan.Value.Segments)
				{
					totalFlightTime += seg.Timespan_Seconds;
				}
				//calculate final DateTime of flight plan
				DateTime planFinalDateTime = planInitialDT.AddSeconds(totalFlightTime);
				//if relativeTo's DateTime is between the plan's initial and final DateTime,
				//add it to activeFlights array
				if ((DateTime.Compare(relativeToDT, planInitialDT) >= 0) &&
					(DateTime.Compare(relativeToDT, planFinalDateTime) <= 0))
				{
					//determine Longitude and Latitude
					List<double> location = calcLongLat(plan, planInitialDT, relativeToDT);
					activeFlights.Add(new Flights()
					{
						Flight_ID = plan.Key,
						Latitude = location[0],
						Longitude = location[1],
						Passengers = plan.Value.Passengers,
						Company_Name = plan.Value.Company_Name,
						Date_Time = relativeTo,
						Is_External = isExternal
					});
				}
/*					activeFlights.Add(new Flights(plan, isExternal, relativeTo, planInitialDT,
						relativeToDT));*/
			}
			return activeFlights;
		}

		public List<Flights> GetExternalInternal(string relativeTo, bool isExternal)
		{
			List<Flights> allActives = GetActiveInternals(relativeTo, isExternal); //internal flights
			List<Flights> externals = new List<Flights>();
			foreach (var server in ServersController.servers)
			{
				string strurl = String.Format(server.Value + "/api/Flights?relative_to=" + relativeTo);
				WebRequest request = WebRequest.Create(strurl);
				request.Method = "GET";
				HttpWebResponse response = null;
				response = (HttpWebResponse)request.GetResponse();
				string strResult = null;
				using (Stream stream = response.GetResponseStream())
				{
					StreamReader sr = new StreamReader(stream);
					strResult = sr.ReadToEnd();
					sr.Close();
				}
				//desirialize
				externals = JsonConvert.DeserializeObject<List<Flights>>(strResult);
				allActives.AddRange(externals);
			}
			return allActives;
		}

	public bool DeleteFlight(string id)
		{
			if (FlightPlanController.plansDict.ContainsKey(id))
			{
				FlightPlanController.plansDict.Remove(id);
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
