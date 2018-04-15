using CSharpChainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpChainServer
{
	public class NodeServices
	{
		private Blockchain blockchain;

		public List<string> Nodes
		{
			get
			{
				return blockchain.Nodes;
			}
		}

		public NodeServices(Blockchain Blockchain)
		{
			this.blockchain = Blockchain;
		}


		public void AddNode(string nodeUrl)
		{
			if (!blockchain.Nodes.Contains(nodeUrl))
			{
				blockchain.Nodes.Add(nodeUrl);
			}
		}

		public void RemoveNode(string nodeUrl)
		{
			blockchain.Nodes.Remove(nodeUrl);
		}
	}
}
