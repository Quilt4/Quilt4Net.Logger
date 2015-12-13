using System;
using System.Security.Cryptography;
using System.Text;

namespace Quilt4Net.Core
{
    internal static class HelperExtension
    {
        public static string ToMd5Hash(this string input)
        {
            var keyBytes = Encoding.UTF8.GetBytes(string.Empty);
            var hashAlgorithm = new HMACSHA1(keyBytes);
            byte[] dataBuffer = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);
            var response = Convert.ToBase64String(hashBytes);
            return response;

            //TODO: Move this code to .NET override class
            //var inputBytes = Encoding.Default.GetBytes(input);
            //var provider = new MD5CryptoServiceProvider();
            //var hash = provider.ComputeHash(inputBytes);
            //return hash.Aggregate(string.Empty, (current, b) => current + b.ToString("X2"));
        }
    }
}