using System;

namespace Quilt4Net.Core.Interfaces
{
    public class InstanceCreatedEventArgs : EventArgs
    {
        public InstanceCreatedEventArgs(IQuilt4NetClient client, int totalNumberOfInstances)
        {
            Client = client;
            TotalNumberOfInstances = totalNumberOfInstances;
        }

        public IQuilt4NetClient Client { get; }
        public int TotalNumberOfInstances { get; }
    }
}