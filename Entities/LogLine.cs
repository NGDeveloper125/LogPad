using Microsoft.Extensions.Logging;
using System;

namespace LogPad.Entities;

public record LogLine(string SourceFileName, Guid LogId, DateTime? Timestamp, LogLevel LogLevel, string ComponentName, string ClassName, string LogMessage);