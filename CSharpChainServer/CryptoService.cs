using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CSharpChainModel
{
    public static class CryptoService
    {
		public static Byte[] GetHash(Byte[] input)
		{
			Byte[] hashBytes;
			using (var algorithm = SHA256.Create())
			{
				hashBytes = algorithm.ComputeHash(input);
			}
			return hashBytes;
		}
	}
}
