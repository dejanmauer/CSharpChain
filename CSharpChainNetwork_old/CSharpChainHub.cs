using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpChainNetwork
{
    public class CSharpChainHub : Hub
    {
		public void NodeRegister(string url)
		{
			// Call the nodesnew method to tell all clients about new node.
			Clients.All.InvokeAsync("noderegister", url);
		}

		public void NodeUnregister(string url)
		{
			// Call the nodesnew method to tell all clients about new node.
			Clients.All.InvokeAsync("noderegister", url);
		}

	}
}
