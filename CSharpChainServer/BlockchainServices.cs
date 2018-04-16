using CSharpChainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpChainServer
{
	public class BlockchainServices
	{
		public Blockchain Blockchain
		{
			get
			{
				return blockchain;
			}

		}

		public object Program { get; private set; }

		private Blockchain blockchain;

		public BlockchainServices()
		{
			// generate initial blockchain with genesis block
			blockchain = new Blockchain();

			// calculate hash of genesis block
			Block genesisBlock = blockchain.Chain[0];
			BlockServices blockServices = new BlockServices(genesisBlock);
			string genesisBlockHash = blockServices.BlockHash();
			blockchain.Chain[0].Hash = genesisBlockHash;
		}

		public void UpdateWithLongestBlockchain ()
		{
			string longestBlockchainNode = "";
			int maxBlockchainLength = 0;

		}


		public Block LatestBlock()
		{
			return blockchain.Chain.Last();
		}

		public Block Block(int index)
		{
			return blockchain.Chain[index];
		}

		public int BlockchainLength()
		{
			return blockchain.Chain.Count();
		}

		public void AddTransaction(Transaction transaction)
		{
			blockchain.PendingTransactions.Add(transaction);
		}

		public List<Transaction> PendingTransactions()
		{
			return blockchain.PendingTransactions;
		}

		public Block MineBlock(string miningRewardAddress)
		{
			// add mining reward transaction to block
			Transaction trans = new Transaction("SYSTEM", miningRewardAddress, blockchain.MiningReward, "Mining reward");
			blockchain.PendingTransactions.Add(trans);

			Block block = new Block(DateTime.Now, blockchain.PendingTransactions, LatestBlock().Hash);
			var blockServices = new BlockServices(block);
			blockServices.MineBlock(blockchain.Difficulty);
			blockchain.Chain.Add(block);

			//clear pending transactions (all pending transactions are in a block
			blockchain.PendingTransactions = new List<Transaction>();
			return block;
		}

		public bool isBlockchainValid()
		{
			for (long i = 1; i < blockchain.Chain.LongCount(); i++)
			{
				Block currentBlock = blockchain.Chain[(int)i];
				Block previousBlock = blockchain.Chain[(int)i - 1];

				BlockServices blockServices = new BlockServices(currentBlock);
				if (currentBlock.Hash != blockServices.BlockHash())
				{
					return false;
				}

				if (currentBlock.PreviousHash != previousBlock.Hash)
				{
					return false;
				}
				blockServices = null;
			}
			return true;
		}

		public decimal Balance(string address)
		{
			decimal balance = 0;

			foreach (Block block in blockchain.Chain)
			{
				foreach (Transaction transaction in block.Transactions)
				{
					if (transaction.SenderAddress == address)
					{
						balance = balance - transaction.Amount;
					}

					if (transaction.ReceiverAddress == address)
					{
						balance = balance + transaction.Amount;
					}
				}
			}
			return balance;
		}



	}
}
