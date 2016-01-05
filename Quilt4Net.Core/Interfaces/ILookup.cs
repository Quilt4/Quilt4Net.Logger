namespace Quilt4Net.Core.Interfaces
{
    public interface ILookup
    {
        IApplicationLookup AplicationLookup { get; }
        IMachineLookup MachineLookup { get; }
        IUserLookup UserLookup { get; }
    }
}