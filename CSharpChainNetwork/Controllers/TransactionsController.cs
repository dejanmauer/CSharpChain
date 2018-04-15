using CSharpChainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CSharpChainNetwork.Controllers
{
	public class TransactionsController : ApiController 
	{
		[HttpGet]
		public string Ping()
		{
			return "  Transactions Contoller Ping";
		}

		[HttpPost]
		public void Add(Transaction Transaction)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("");
			Console.WriteLine($"  API notification received: transactions-add {Transaction.Description} ");
			var foundTransaction = Program.blockchainServices.PendingTransactions().Where(pt => pt.Description == Transaction.Description).FirstOrDefault();
			if (foundTransaction==null)
			{
				Console.WriteLine($"    Transaction: {Transaction.Amount} from {Transaction.SenderAddress} to {Transaction.ReceiverAddress}.");
				Program.blockchainServices.AddTransaction(Transaction);
			} else
			{
				Console.WriteLine($"    Transaction already exists on this node.");
			}
			Console.ResetColor();
			Program.ShowCommandLine();
		}
	}
}
