﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightControlWeb.Controllers
{
	[Route("api/[controller]")]
	public class FlightPlanController : Controller
	{
		private IFlightPlanManager manager = new FlightPlanManager();
		public static Dictionary<string, FlightPlan> plansDict = new Dictionary<string, FlightPlan>();

		// GET: api/<controller>
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<controller>/5
		[HttpGet("{id}")]
		public Object Get(string id)
		{
			//if it's in our dictionary return it, otherwise look for it in external servers
			FlightPlan flightPlan;
			if (plansDict.TryGetValue(id, out flightPlan))
			{
				return flightPlan;
			} else
			{
				//TODO change this to return a flight from external servers
				//TODO if not found then maybe return 303
				return null;
			}
		}

		// POST api/<controller>
		[HttpPost]
		public ActionResult<string> Post([FromBody]FlightPlan flightPlan)
		{
            manager.AddPlan(flightPlan, plansDict);
			//add if else and other return options
			return Ok("Flight plan added successfully");
		}

		// PUT api/<controller>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/<controller>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}