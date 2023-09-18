using System.Security.Cryptography;
using System.Text;

namespace Quilt4Net.Internals;

internal static class HashExtensions
{
    public static string ToHash(this string item)
    {
        if (string.IsNullOrEmpty(item))
        {
            return null;
        }

        var data = Encoding.UTF8.GetBytes(item);
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(data);
        return Convert.ToBase64String(hash);
    }
}