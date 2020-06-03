﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightControlWeb.Controllers
{
	[Route("api/[controller]")]
	public class FlightsController : Controller
	{
		private FlightsManager flightsManager = new FlightsManager();
		// GET: api/<controller>
		[HttpGet]
		public Object GetActiveFlights([FromQuery(Name = "relative_to")] string relativeTo)
		{
			//validity check
			string format = "yyyy-MM-ddTHH:mm:ssZ";
			DateTime dateTime;
			if (!DateTime.TryParseExact(relativeTo, format, CultureInfo.InvariantCulture,
				DateTimeStyles.None, out dateTime))
			{
				return "relative_to format should be yyyy-MM-ddTHH:mm:ssZ";
			}

			string request = Request.QueryString.Value;
			bool isExternal = request.Contains("sync_all");
			List<Flights> actives = new List<Flights>();
			if (!isExternal)
			{
				actives = flightsManager.GetActiveInternals(relativeTo, isExternal);
			}
			else
			{
				actives = flightsManager.GetExternalInternal(relativeTo, isExternal);				
			}
			if (!actives.Any())
			{
				return "No active flights in this date and time";
			}
			return actives;
		}

		// GET api/<controller>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<controller>
		[HttpPost]
		public void Post([FromBody]string value)
		{
		}

		// PUT api/<controller>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/<controller>/5
		[HttpDelete("{id}")]
		public ActionResult<string> Delete(string id)
		{
			if (flightsManager.DeleteFlight(id))
			{
				return Ok("Flight no. " + id + " deleted successfully");
			}
			else
			{
				return BadRequest("Id does not exist in server");
			}
		}
	}
}
