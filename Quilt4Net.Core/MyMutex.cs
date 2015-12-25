using System;
using System.Threading;
using System.Threading.Tasks;

namespace Quilt4Net.Core
{
    class MyMutex
    {
        private readonly string _key;

        public MyMutex(string key)
        {
            _key = key;
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            //System.Threading.Mutex v = new Mutex();
            //Mutex mutex;
            //try
            //{
            //    mutex = new Mutex(false, _key);
            //}
            //catch (Exception exception)
            //{
            //    mutex = Mutex.OpenExisting(_key);
            //}

            bool createdNew;
            using (var mutex = new Mutex(false, _key, out createdNew))
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

        //public T Execute<T>(Func<T> func)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Execute(Action action)
        //{
        //    Execute(() =>
        //    {
        //        action();
        //        return true;
        //    });
        //}
    }
}