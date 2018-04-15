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
	public class BlockchainController : Controller
	{
		public void Mine(string mineAddress)
		{
			CSharpChain.BlockChain blockChain = new CSharpChain.BlockChain("");
			blockChain.MinePendingTransations(mineAddress);

			// update all nodes about new block
			var network = new NetworkController();

			var client = new HttpClient();
			// call url/node/events/blockchain/update
		}

    }
}
