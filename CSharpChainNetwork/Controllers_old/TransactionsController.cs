using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CSharpChainNetwork.Controllers
{
	public class TransactionsController : ApiController
	{

		public class AddParams
		{
			public string SenderUrl { get; set; }
			public string SenderAddress { get; set; }
			public string ReceiverAddress { get; set; }
			public string Amount { get; set; }
			public string Description { get; set; }
		}

		[HttpPost]
		public void Add([FromBody]AddParams parameters)
		{
			Console.WriteLine($"--WebAPI: Url {parameters.SenderUrl} called Transactions/Add.");
			BlockchainServices.TransactionsAdd(parameters.SenderAddress, parameters.ReceiverAddress, Decimal.Parse(parameters.Amount), parameters.Description, false);
		}

	}
}