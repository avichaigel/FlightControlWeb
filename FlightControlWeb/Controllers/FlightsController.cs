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
		public static Dictionary<string, string> externalActiveFlights = new Dictionary<string, string>();
		// GET: api/<controller>
		[HttpGet]
		public async Task<Object> GetActiveFlights([FromQuery(Name = "relative_to")] string relativeTo)
		{
			if (!flightsManager.IsValidDateTime(relativeTo))
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
				try
				{
					actives = await flightsManager.GetExternalInternal(relativeTo, isExternal);
				} catch (Exception e)
				{
					return e.Message;
				}
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
