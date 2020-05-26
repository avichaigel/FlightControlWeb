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

		public Flights[] GetActiveFlights(string relative_to)
		{
			IEnumerable<FlightPlan> plans = FlightPlanController.plansDict.Values;
			foreach (var plan in plans)
			{
				int totalFlightTime = 0;
				string[] date_time = plan.Initial_Location.Date_Time.Split("T");
				//TODO maybe trim the 'Z' from date_time[1]
				foreach (var seg in plan.Segments)
				{
					totalFlightTime += seg.Timespan_Seconds;
				}

			}
		}
	}
}
