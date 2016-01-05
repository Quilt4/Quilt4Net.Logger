using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Handlers
{
    internal class HashHandler : IHashHandler
    {
        public string ToMd5Hash(string input)
        {
            var inputBytes = Encoding.Default.GetBytes(input);
            var provider = new MD5CryptoServiceProvider();
            var hash = provider.ComputeHash(inputBytes);
            var result = hash.Aggregate(string.Empty, (current, b) => current + b.ToString("X2"));
            return result;
        }
    }
}