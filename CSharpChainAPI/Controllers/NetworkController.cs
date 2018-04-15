using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSharpChainAPI.Models;
using Newtonsoft.Json;

namespace CSharpChainAPI.Controllers
{
    [Route("api/[controller]")]
    public class NetworkController : Controller
    {

		private List<Node> Nodes = new List<Node>();

		// POST api/values
		[HttpPost]
		public void Register([FromBody]string newUrl)
		{
			Node node = new Node
			{
				Url = newUrl
			};
			Nodes.Add(node);
		}

		public string List()
		{
			var json = JsonConvert.SerializeObject(Nodes);
			return json;
		}


    }
}
