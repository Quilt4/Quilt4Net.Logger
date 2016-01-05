using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.Lookups
{
    public class Lookup : ILookup
    {
        public Lookup(IApplicationLookup aplicationLookup, IMachineLookup machineLookup, IUserLookup userLookup)
        {
            AplicationLookup = aplicationLookup;
            MachineLookup = machineLookup;
            UserLookup = userLookup;
        }

        public IApplicationLookup AplicationLookup { get; }
        public IMachineLookup MachineLookup { get; }
        public IUserLookup UserLookup { get; }
    }
}