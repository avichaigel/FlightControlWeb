using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
	public class FlightPlanManager : IFlightPlanManager
	{
		public FlightPlanManager()
		{
		}

		public static List<string> usedIDs = new List<string>();
		public string NewID()
		{
			StringBuilder tempPass = new StringBuilder();
			Random random = new Random();
			string password;
			//create new password which consists of two upper case letters and 4 ints
			//if it's already in use, create a new one and override the old one
			do
			{
				for (int i = 0; i < 2; i++)
				{
					char c = Convert.ToChar(random.Next(65, 90));
					tempPass.Append(c);
				}
				password = tempPass.Append(random.Next(1000, 9999)).ToString();
			} while (usedIDs.Contains(password));

			usedIDs.Add(password);
			return password;
		}

		public void AddPlan(FlightPlan flightPlan, Dictionary<string, FlightPlan> plansDict)
		{
			plansDict.Add(NewID(), flightPlan);

/*			IDs id = new IDs();
			flightPlan.ID = id.NewID(); //add id

			//add segments and Initial_Location to their dbs
			flightPlan.Segments.ForEach(x => {
				x.id = id.NewID();
				context.Segments.Add(x);
				});

			flightPlan.Initial_Location.id = id.NewID();
			context.InitialLocs.Add(flightPlan.Initial_Location);
			//add to the DB
			context.FlightPlans.Add(flightPlan);
			context.SaveChangesAsync();*/
		}
	}
}
