using FlightControlWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
	public class ServersManager
	{
		public ServersManager() { }

		public void AddServer(Server newServer, Dictionary<string, string> servers)
		{
			servers.Add(newServer.Server_ID, newServer.Server_URL);
		}

		public bool DeleteServer(string id)
		{
			if (ServersController.servers.ContainsKey(id))
			{
				ServersController.servers.Remove(id);
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
