using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightControlWeb.Controllers
{
	[Route("api/[controller]")]
	public class ServersController : Controller
	{
		private ServersManager srvManager = new ServersManager();
		public static Dictionary<string, string> servers = new Dictionary<string, string>();

		// GET: api/<controller>
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return servers.Values;
		}

		// GET api/<controller>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<controller>
		[HttpPost]
		public ActionResult<string> Post([FromBody]Server newServer)
		{
			srvManager.AddServer(newServer, servers);
			//TODO add if else and other return options
			return Ok("Flight plan added successfully");
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
			if (srvManager.DeleteServer(id))
			{
				return Ok("Flight deleted successfully");
			}
			else
			{
				return BadRequest("Id does not exist in server");
			}
		}
	}
}
