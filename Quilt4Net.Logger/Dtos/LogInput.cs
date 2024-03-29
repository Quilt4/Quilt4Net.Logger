﻿namespace Quilt4Net.Dtos;

public record LogInput
{
    public string CategoryName { get; init; }
    public int LogLevel { get; init; }
    public string Message { get; init; }
    public string AppDataKey { get; init; }
    public string SessionDataKey { get; init; }
    public LogDataItem[] Data { get; init; }
    public long? TimeInTicks { get; init; }
}