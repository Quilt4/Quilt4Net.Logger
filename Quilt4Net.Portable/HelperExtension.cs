using System;

namespace Quilt4Net.Core
{
    internal static class HelperExtension
    {
        public static string ToMd5Hash(this string input)
        {
            throw new NotImplementedException();
            //var inputBytes = Encoding.Default.GetBytes(input);
            //var provider = new MD5CryptoServiceProvider();
            //var hash = provider.ComputeHash(inputBytes);
            //return hash.Aggregate(string.Empty, (current, b) => current + b.ToString("X2"));
        }
    }
}