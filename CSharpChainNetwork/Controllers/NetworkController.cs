using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CSharpChainNetwork.Controllers
{
	public class NetworkController : ApiController
	{
		[HttpGet]
		public string Ping()
		{
			return "  Network Contoller Ping";
		}

		[HttpPost]
		public void Register([FromBody]string NewNodeUrl)
		{
			Console.ForegroundColor = ConsoleColor.Cyan ;
			Console.WriteLine("");
			Console.WriteLine($"  API notification received: node-register {NewNodeUrl} ");
			Console.WriteLine("    Register new node: " + NewNodeUrl);
			Program.nodeServices.AddNode(NewNodeUrl);
			Console.ResetColor();
			Program.ShowCommandLine();
		}

		[HttpPost]
		public void Unregister(string RemoveNodeUrl)
		{
			Console.ForegroundColor = ConsoleColor.Cyan ;
			Console.WriteLine("");
			Console.WriteLine($"  API notification received: node-unregister {RemoveNodeUrl} ");
			Console.WriteLine("    Unregister new node: " + RemoveNodeUrl);
			Program.nodeServices.RemoveNode(RemoveNodeUrl);
			Console.ResetColor();
			Program.ShowCommandLine();
		}


	}
}
