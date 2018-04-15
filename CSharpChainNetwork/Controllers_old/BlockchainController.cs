using CSharpChainModel;
using CSharpChainServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
	public class BlockchainController : ApiController
	{

		[HttpGet]
		public long Length()
		{
			return Program.blockchainServices.BlockchainLength();
		}

		[HttpGet]
		public Block Block(int id)
		{
			return Program.blockchainServices.Block(id);
		}

		public class NewBlockModel
		{
			public Block newBlock { get; set; }
			public int newBlockIndex { get; set; }
			public string senderUrl { get; set; }
		}
		[HttpPost]
		public bool NewBlock([FromBody]NewBlockModel newBlock)
		{
			//NewBlockModel newBlockObject = JsonConvert.DeserializeObject<NewBlockModel>(newBlock.ToString());
			if (BlockchainServices.BlockChainLength() + 1 == newBlock.newBlockIndex)
			{
				Program.blockchainServices.blo .BlockChainAddBlock(newBlock.newBlock);

				// remove from pending transactions
				List<int> RemovePending = new List<int>();
				for (int i = 0; i < newBlock.newBlock.Transactions.Count; i++)
				{
					var block = Program.blockchainServices.Blockchain.PendingTransactions.Where(pt =>
						pt.Amount == newBlock.newBlock.Transactions[i].Amount &&
					pt.Description == newBlock.newBlock.Transactions[i].Description &&
					pt.ReceiverAddress == newBlock.newBlock.Transactions[i].ReceiverAddress &&
					pt.SenderAddress == newBlock.newBlock.Transactions[i].SenderAddress).FirstOrDefault();

					if (block != null)
					{
						RemovePending.Add(i);
					};
				};

				for (int i = RemovePending.Count - 1; i >= 0; i--)
				{
					Program.blockchainServices.Blockchain.PendingTransactions.RemoveAt(RemovePending[i]);
				}

			}
			return true;
		}

		[HttpGet]
		public Blockchain Blockchain()
		{
			return Program.blockchainServices.Blockchain;
		}


	}
}