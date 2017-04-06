namespace Quilt4Net.Core.Interfaces
{
    public interface IQuilt4NetClient
    {
        IActions Actions { get; }
        IInformation Information { get; }
        IClient Client { get; }
        IConfiguration Configuration { get; }
    }
}