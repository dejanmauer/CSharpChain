
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpChainModel
{
	public class Block
	{
		public string PreviousHash;
		public DateTime TimeStamp;
		public List<Transaction> Transactions;
		public string Hash;
		public long Nonce;

		public Block(DateTime timeStamp, List<Transaction> transactions, string previousHash)
		{
			this.TimeStamp = timeStamp;
			this.PreviousHash = previousHash;
			this.Transactions = transactions;
			this.Hash = "";
			this.Nonce = 0;
		}



	}
}
