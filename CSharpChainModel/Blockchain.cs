using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpChainModel
{
	public class Blockchain
	{
		public List<Block> Chain;
		public List<string> Nodes;
		public int Difficulty;
		public List<Transaction> PendingTransactions;
		public int MiningReward;

		public Blockchain()
		{
			this.Chain = new List<Block>();
			this.Chain.Add(CreateGenesisBlock());

			this.Nodes = new List<string>();

			this.Difficulty = 4;
			this.PendingTransactions = new List<Transaction>();
			this.MiningReward = 100;
		}

		private Block CreateGenesisBlock()
		{
			Block genesis = new Block(new DateTime(2000, 01, 01), new List<Transaction>(), "0");
			return genesis;
		}


	}
}
