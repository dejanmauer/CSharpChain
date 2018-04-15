using CSharpChainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CSharpChainNetwork.Controllers
{
	public class BlockchainController : ApiController
	{
		[HttpGet]
		public string Ping()
		{
			return "  Blockchain Contoller Ping";
		}

		[HttpGet]
		public int Length()
		{
			return Program.blockchainServices.BlockchainLength();
		}

		[HttpGet]
		public Block GetBlock(int Id)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("");
			Console.WriteLine($"  API notification received: blockchain-getBlock {Id} ");
			Console.WriteLine("    Get block.");

			var block = Program.blockchainServices.Block(Id);

			Console.ResetColor();
			Program.ShowCommandLine();

			return block;
		}

		[HttpGet]
		public Blockchain GetBlockchain()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("");
			Console.WriteLine($"  API notification received: blockchain-getBlockchain() ");
			Console.WriteLine("    Get entire blockchain.");

			var blockChain = Program.blockchainServices.Blockchain;

			Console.ResetColor();
			Program.ShowCommandLine();

			return blockChain;
		}

		[HttpPost]
		public void NewBlock(Block NewBlock)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("");
			Console.WriteLine($"  API notification received: blockchain-newBlock {NewBlock.Hash} ");
			Console.WriteLine("    Add new block.");

			Program.blockchainServices.Blockchain.Chain.Add(NewBlock);
			// check if the blockchain is valid
			if (!Program.blockchainServices.isBlockchainValid())
			{
				Console.WriteLine("    Blockchain with new block added is not valid. Undoing block.");
				Program.blockchainServices.Blockchain.Chain.Remove(NewBlock);
				Console.WriteLine("    Try refresing with the longest blockchain");
			}

			Console.ResetColor();
			Program.ShowCommandLine();
		}
	}
}
