using System.Collections.Generic;
using System.Threading;

namespace Quilt4Net.Internals.Standard
{
    internal static class DictionaryExtensions
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key)) return false;

            try
            {
                _semaphore.Wait();
                if (dictionary.ContainsKey(key)) return false;
                dictionary.Add(key, value);
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}