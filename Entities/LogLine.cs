using Microsoft.Extensions.Logging;
using System;

namespace LogPad.Entities;

public record LogLine
{
    public string SourceFileName { get; init; }
    public int LogId{ get; init; }
    public DateTime Timestamp { get; init; }
    public LogLevel LogLevel { get; init; }
    public string ComponentName { get; init; }
    public string ClassName { get; init; }
    public string LogMessage { get; init; }
}
