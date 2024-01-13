using Microsoft.Extensions.Logging;

namespace Quilt4Net.Dtos;

internal interface IConfigurationData
{
    LogLevel MinLogLevel { get; }
}