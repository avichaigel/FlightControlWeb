using FlightControlWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
	public class FlightsManager
	{
		public FlightsManager() { }

		public Flights[] GetActiveFlights(string relativeTo, bool isExternal)
		{
			Flights[] activeFlights = null;
			//create a DateTime object out of the relatieTo argument
			DateTime relativeToDateTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(relativeTo));

			//traverse through the flightPlans in our server, and if relativeTo's DateTime is between
			//the plan's initial and final DateTime, add it to activeFlights array
			//IEnumerable<FlightPlan> plans = FlightPlanController.plansDict.Values;
			foreach (var plan in FlightPlanController.plansDict)
			{
				int totalFlightTime = 0;
				//create a DateTime object out of the flight plan's Date_Time string
				DateTime planInitialDateTime = 
					TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(plan.Value.Initial_Location.Date_Time));
				//calculate total flight time in seconds
				foreach (var seg in plan.Value.Segments)
				{
					totalFlightTime += seg.Timespan_Seconds;
				}
				//calculate final DateTime of flight plan
				DateTime planFinalDateTime = planInitialDateTime.AddSeconds(totalFlightTime);
				//if relativeTo's DateTime is between the plan's initial and final DateTime,
				//add it to activeFlights array
				if ((DateTime.Compare(relativeToDateTime, planInitialDateTime) >= 0) &&
					(DateTime.Compare(relativeToDateTime, planFinalDateTime) <= 0))
				{
					activeFlights.Append<Flights>(new Flights(plan, isExternal));
				}
			}
			return activeFlights;
		}
	}
}
