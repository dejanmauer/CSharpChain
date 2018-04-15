using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSharpChainAPI.Models;
using Newtonsoft.Json;

namespace CSharpChainAPI.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {

		public void Add(string fromAddress, string toAddress, decimal amount)
		{
			CSharpChain.Transaction transaction = new CSharpChain.Transaction(fromAddress, toAddress, amount, "Transaction");
		}



    }
}
