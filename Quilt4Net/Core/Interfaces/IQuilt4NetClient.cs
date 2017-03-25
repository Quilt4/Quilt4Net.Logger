namespace Quilt4Net.Core.Interfaces
{
    public interface IQuilt4NetClient
    {
        IActions Actions { get; }
        IInformation Information { get; }
        IWebApiClient WebApiClient { get; }
        IConfiguration Configuration { get; }
    }
}