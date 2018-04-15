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
	public class ValuesController : ApiController
	{
		[HttpPost]
		public void Add([FromBody]string url)
		{
			BlockchainServices.NodesAddNodeAsync(url);
		}

		[HttpPost]
		public void Remove([FromBody]string url)
		{
			BlockchainServices.NodesRemoveNode(url);
		}

	}
}