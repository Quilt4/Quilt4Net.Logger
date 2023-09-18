namespace Quilt4Net;

public interface ILoggingDefaultData
{
    ILoggingDefaultData AddData(string key, object value);
    IDictionary<string, object> GetData();
}