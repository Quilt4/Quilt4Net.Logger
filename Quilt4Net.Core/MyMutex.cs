using System;
using System.Threading;
using System.Threading.Tasks;

namespace Quilt4Net.Core
{
    class MyMutex2
    {
        private readonly string _key;

        public MyMutex2(string key)
        {
            _key = key;
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            bool createdNew;
            using (var mutex = new Mutex(false, _key + "_", out createdNew))
            {
                var completed = false;
                try
                {
                    completed = mutex.WaitOne();
                    return await func();
                }
                finally
                {
                    if (completed)
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }
        }

        public async Task ExecuteAsync(Func<Task> func)
        {
            await ExecuteAsync(async () =>
            {
                await ExecuteAsync(func);
                return true;
            });
        }
    }
}