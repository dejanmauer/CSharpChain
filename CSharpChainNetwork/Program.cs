using CSharpChainModel;
using CSharpChainServer;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSharpChainNetwork
{
	static class Program
	{

		static string baseAddress;
		public static BlockchainServices blockchainServices;
		public static NodeServices nodeServices;

		static bool useNetwork = true;

		public static void ShowCommandLine()
		{
			Console.WriteLine("");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("CSharpChain> ");
			Console.ResetColor();
		}

		static void Main(string[] args)
		{

			baseAddress = args[0];
			if (!baseAddress.EndsWith("/")) baseAddress += "/";

			// Start OWIN host 
			using (WebApp.Start<Startup>(url: baseAddress))
			{
				Console.WriteLine("");
				Console.WriteLine("CSharpChain Blockchain // dejan@mauer.si // www.mauer.si");
				Console.WriteLine("----------------------------------------");
				Console.WriteLine("This CSharpChain node is running on " + baseAddress);
				Console.WriteLine("Type 'help' if you are not sure what to do ;)");

				blockchainServices = new BlockchainServices();
				nodeServices = new NodeServices(blockchainServices.Blockchain);

				string commandLine;
					do
				{
					ShowCommandLine();

					commandLine = Console.ReadLine().ToLower();
					commandLine += " ";
					var command = commandLine.Split(' ');

					switch (command[0])
					{
						case "quit":
						case "q":
							commandLine = "q";
							break;

						case "help":
						case "?":
							CommandHelp();
							break;

						case "node-add":
						case "na":
							// if param1 is numeric then translate to localhost port
							if (command[1].All(char.IsDigit)) command[1] = "http://localhost:" + command[1];
							CommandNodeAdd(command[1]);
							break;

						case "node-remove":
						case "nr":
							// if param1 is numeric then translate to localhost port
							if (command[1].All(char.IsDigit)) command[1] = "http://localhost:" + command[1];
							CommandNodeRemove(command[1]);
							break;

						case "nodes-list":
						case "nl":
							CommandListNodes(nodeServices.Nodes);
							break;

						case "transactions-add":
						case "ta":
							CommandTransactionsAdd(command[1], command[2], command[3], command[4]);
							break;

						case "transactions-pending":
						case "tp":
							CommandListPendingTransactions(blockchainServices.Blockchain.PendingTransactions);
							break;

						case "blockchain-mine":
						case "bm":
							CommandBlockchainMine(command[1]);
							break;

						case "bv":
						case "blockchain-valid":
							CommandBlockchainValidity();
							break;

						case "blockchain-length":
						case "bl":
							CommandBlockchainLength();
							break;

						case "block":
						case "b":
							CommandBlock(int.Parse(command[1]));
							break;

						case "balance-get":
						case "bal":
							CommandBalance(command[1]);
							break;

						case "blockchain-update":
						case "update":
						case "bu":
							CommandBlockchainUpdate();
							break;

						default:
							Console.WriteLine("Ups! I don't understand...");
							Console.WriteLine("");
							break;
					}
				} while (commandLine != "q");
			}
		}
		#region Commands 

		static void CommandListNodes(List<string> Nodes)
		{
			foreach (string node in Nodes)
			{
				Console.WriteLine($"  Node: {node}");
			}
			Console.WriteLine("");
		}

		static void CommandListPendingTransactions(List<Transaction> PendingTransactions)
		{
			foreach (Transaction transaction in PendingTransactions)
			{
				Console.WriteLine($"  Transaction: {transaction.Amount} from {transaction.SenderAddress} to {transaction.ReceiverAddress} with description {transaction.Description}");
			}
			Console.WriteLine("");
		}

		static void CommandNodeAdd(string NodeUrl)
		{
			if (!NodeUrl.EndsWith("/")) NodeUrl += "/";
			nodeServices.AddNode(NodeUrl);
			Console.WriteLine($"  Node {NodeUrl} added to list of blockchain peer nodes.");
			if (useNetwork)
			{
				NetworkRegister(NodeUrl);
				CommandBlockchainUpdate();
			}
			Console.WriteLine("");
		}

		static void CommandHelp()
		{
			Console.WriteLine("Commands:");
			Console.WriteLine("h, help = list of commands.");
			Console.WriteLine("q, quit = exit the program.");
			Console.WriteLine("na, node-add [url] = connect current node to other node.");
			Console.WriteLine("nr, node-remove [url] = disconnect current node from other node.");
			Console.WriteLine("nl, nodes-list = list connected nodes.");
			Console.WriteLine("ta, transaction-add [senderAddress] [receiverAddress] [Amount] [Description] = create new transaction.");
			Console.WriteLine("np, transactions-pending = list of transactions not included in block.");
			Console.WriteLine("bm, blockchain-mine [rewardAddress] = create block from pending transactions,");
			Console.WriteLine("bv, blockchain-valid = Validates blockchain.");
			Console.WriteLine("bl, blockchain-length = number of blocks in blockchain.");
			Console.WriteLine("b, block [index] = list info about specified block.");
			Console.WriteLine("bu, update, blockchain-update = update blockchain to the longest in network.");
			Console.WriteLine("bal, balance-get [address] = get balance for specified address.");
			Console.WriteLine();
			Console.WriteLine("Email me: dejan@mauer.si");
			Console.WriteLine();

		}

		static void CommandBlockchainUpdate()
		{
			Console.WriteLine($"  Updating blockchain with the longest found on network.");
			if (useNetwork)
			{
				NetworkBlockchainUpdate();
			}
			Console.WriteLine("");
		}

		static void CommandNodeRemove(string NodeUrl)
		{
			if (!NodeUrl.EndsWith("/")) NodeUrl += "/";
			nodeServices.RemoveNode(NodeUrl);
			Console.WriteLine($"  Node {NodeUrl} removed to list of blockchain peer nodes.");
			Console.WriteLine("");
		}

		static void CommandTransactionsAdd(string SenderAddress, string ReceiverAddress, string Amount, string Description)
		{
			Transaction transaction = new Transaction(SenderAddress, ReceiverAddress, Decimal.Parse(Amount), Description);
			blockchainServices.AddTransaction(transaction);
			Console.WriteLine($"  {Amount} from {SenderAddress} to {ReceiverAddress} transaction added to list of pending transactions.");
			Console.WriteLine("");

			if (useNetwork)
			{
				NetworkTransactionAdd(transaction);
			}
			Console.WriteLine("");
		}

		static void CommandBlockchainMine(string RewardAddress)
		{
			Console.WriteLine($"  Mining new block... Difficulty {blockchainServices.Blockchain.Difficulty}.");
			blockchainServices.MineBlock(RewardAddress);
			Console.WriteLine($"  Block has been added to blockchain. Blockhain length is {blockchainServices.BlockchainLength().ToString()}.");
			Console.WriteLine("");
			if (useNetwork)
			{
				NetworkBlockchainMine(blockchainServices.LatestBlock());
			}
			Console.WriteLine("");
		}

		static void CommandBlockchainValidity()
		{
			var result = blockchainServices.isBlockchainValid();
			if (result == true)
			{
				Console.WriteLine($"  Blockchain is valid.");
			}
			else
			{
				Console.WriteLine($"  Blockchain is invalid.");
			}
			Console.WriteLine("");
		}

		static void CommandBlockchainLength()
		{
			var length = blockchainServices.BlockchainLength();
			Console.WriteLine($"  Blockchain length is {length.ToString()}.");
			Console.WriteLine("");
		}

		static void CommandBlock(int Index)
		{
			var block = blockchainServices.Block(Index);
			Console.WriteLine($"  Block {Index}:");
			Console.WriteLine($"    Hash: {block.Hash}:");
			Console.WriteLine($"    Nonce: {block.Nonce}:");
			Console.WriteLine($"    Previous hash: {block.PreviousHash}:");
			Console.WriteLine($"    #Transactions : {block.Transactions.Count}:");
			Console.WriteLine("");
		}

		static void CommandBalance(string Address)
		{
			var balance = blockchainServices.Balance(Address);
			Console.WriteLine($"  Balance for address {Address} is {balance.ToString()}.");
			Console.WriteLine("");
		}
		#endregion

		#region NetworkSend

		static async void NetworkRegister(string NewNodeUrl)
		{
			// automatically notify node you are registering about this node
			using (var client = new HttpClient())
			{
				try
				{
					Console.WriteLine($"  Initiating API call: calling {NewNodeUrl} node-register {baseAddress}");

					var content = new FormUrlEncodedContent(
						new Dictionary<string, string>
						{
							{"" , baseAddress}
						}
					);

					var response = await client.PostAsync(NewNodeUrl + "api/network/register", content);
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("  " + ex.Message);
					Console.ResetColor();
				}
			}
		}

		static async void NetworkUnregister(string UnregisterNodeUrl)
		{
			// automatically notify node you are registering about this node
			using (var client = new HttpClient())
			{
				try
				{
					Console.WriteLine($"  Initiating API call: calling {UnregisterNodeUrl} node-unregister {baseAddress}");

					var content = new FormUrlEncodedContent(
						new Dictionary<string, string>
						{
							{"" , baseAddress}
						}
					);

					var response = await client.PostAsync(UnregisterNodeUrl + "api/network/unregister", content);
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("  " + ex.Message);
					Console.ResetColor();
				}
			}
		}

		static async void NetworkTransactionAdd(Transaction Transaction)
		{
			// automatically notify node you are registering about this node
			using (var client = new HttpClient())
			{
				try
				{
					// iterate all registered nodes
					foreach (string node in nodeServices.Nodes)
					{
						Console.WriteLine($"  Initiating API call: calling {node} transaction-add {Transaction.Description}");
						var response = await client.PostAsJsonAsync<Transaction>(node + "api/transactions/add", Transaction);
						Console.WriteLine();
					}
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("  " + ex.Message);
					Console.ResetColor();
				}
			}
		}

		static async void NetworkBlockchainUpdate()
		{
			int maxLength = 0;
			string maxNode = "";

			// find the node with the longest blockchain
			using (var client = new HttpClient())
			{
				try
				{

					// iterate all registered nodes
					foreach (string node in nodeServices.Nodes)
					{
						Console.Write($"  Initiating API call: calling {node} blockchain-length.");
						var response = await client.GetAsync(node + "api/blockchain/length");

						int newLength = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

						if (newLength > maxLength)
						{
							maxNode = node;
							maxLength = newLength;
						}
						Console.WriteLine(response);
						Console.WriteLine();
					}
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("  " + ex.Message);
					Console.ResetColor();
				}

				Console.WriteLine($"    Max blockchain length found on {maxNode} with length {maxLength}.");
				if (blockchainServices.BlockchainLength() >= maxLength)
				{
					Console.WriteLine($"    No blockchain found larger than existing one.");
					Console.WriteLine();
					return;
				}


				// get missing blocks
				try
				{
					for (int i = blockchainServices.BlockchainLength(); i < maxLength; i++)
					{
						Block newBlock;
						Console.WriteLine($"      Requesting block {i} from {maxNode}...");
						var response = await client.GetAsync(maxNode + $"api/blockchain/getblock?id={i}");
						newBlock = JsonConvert.DeserializeObject<Block>(await response.Content.ReadAsStringAsync());

						blockchainServices.Blockchain.Chain.Add(newBlock);
						if (!blockchainServices.isBlockchainValid())
						{
							blockchainServices.Blockchain.Chain.Remove(newBlock);
							Console.WriteLine($"    After adding block {i} blockchain is not valid anymore. Canceling...");
							break;
						}

					}

				}
				catch (Exception)
				{

					throw;
				}

				Console.WriteLine($"    Updated!");
				Console.WriteLine();
			}
		}


		static async void NetworkBlockchainMine(Block NewBlock)
		{
			// automatically notify node you are registering about this node
			using (var client = new HttpClient())
			{
				try
				{
					// iterate all registered nodes
					foreach (string node in nodeServices.Nodes)
					{
						Console.WriteLine($"  Initiating API call: calling {node} blockchain-newBlock {NewBlock.Hash}");
						var response = await client.PostAsJsonAsync<Block>(node + "api/blockchain/newblock", NewBlock);
						Console.WriteLine();
					}
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("  " + ex.Message);
					Console.ResetColor();
				}
			}
		}



		#endregion
	}
}

