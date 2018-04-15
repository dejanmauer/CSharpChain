using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpChainModel
{
    public class Transaction
    {
		public string SenderAddress;
		public string ReceiverAddress;
		public decimal Amount;
		public string Description;
		

		public Transaction(string senderAddress, string receiverAddress, decimal amount, string description)
		{
			this.SenderAddress = senderAddress;
			this.ReceiverAddress = receiverAddress;
			this.Amount = amount;
			this.Description = description;
		}

    }
}
