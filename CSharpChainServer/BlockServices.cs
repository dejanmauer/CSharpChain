using CSharpChainModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpChainServer
{
	public class BlockServices
	{

		private Block block;
		
		public BlockServices(Block Block)
		{
			this.block = Block;
		}

		private string ByteArrayToString(byte[] ba)
		{
			return BitConverter.ToString(ba).Replace("-", "");
		}

		public string BlockHash()
		{
			string hashString = "";
			hashString = block.PreviousHash + block.TimeStamp.ToString() + JsonConvert.SerializeObject(block.Transactions, Formatting.Indented) + block.Nonce;
			byte[] hashBytes = Encoding.ASCII.GetBytes(hashString);
			Byte[] hashResult = CryptoService.GetHash(hashBytes);
			return ByteArrayToString(hashResult);
		}

		public void MineBlock(int difficulty)
		{
			string startsWith = "";
			for (int n = 0; n < difficulty; n++)
			{
				startsWith += "0";
			}

			while (block.Hash=="" || block.Hash.Substring(0, difficulty) != startsWith)
			{
				block.Nonce = block.Nonce + 1;
				block.Hash = BlockHash();
			};
		}

	}
}
