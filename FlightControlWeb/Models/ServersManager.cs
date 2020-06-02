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
			if (!servers.ContainsKey(newServer.Server_ID))
			{
				servers.Add(newServer.Server_ID, newServer.Server_URL);
			} else
			{
				throw new ArgumentException("ID is already in use");
			}
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
