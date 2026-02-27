using LogPad.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogPad.Extensions;

public static class StringExtensions
{
    public static List<string> ToLines(this string self)
    { 
        return self.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    public static DateTime? GetTimeStamp(this string self)
    {
        string section = GetLineSection(self, LogLineSection.TimeStamp);
        if (DateTime.TryParse(section, out DateTime timestamp))
        {
            return timestamp;
        }
        else
        {
            return null;
        }
    }

    public static LogLevel GetLogLevel(this string self)
    {
        string section = GetLineSection(self, LogLineSection.LogLevel).Trim(']').Trim('[').ToLower();
        return section switch
        {
            "debug" => LogLevel.Debug,
            "info" => LogLevel.Information,
            "warn" => LogLevel.Warning,
            "error" => LogLevel.Error,
            _ => throw new ArgumentException($"Invalid log level: {section}")
        };
    }

    public static string GetComponentName(this string self)
    {
        return GetLineSection(self, LogLineSection.ComponentName);
    }

    public static string GetClassName(this string self)
    {
        return GetLineSection(self, LogLineSection.ClassName);
    }

    public static string GetLogMessage(this string self)
    {
        return GetLineSection(self, LogLineSection.LogMessage);
    }

    private static string GetLineSection(string line, LogLineSection logLineSection)
    { 
        string[] lineSections = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return logLineSection switch
        {
            LogLineSection.TimeStamp => lineSections[0] + " " + lineSections[1],
            LogLineSection.LogLevel => lineSections[3],
            LogLineSection.ComponentName => lineSections[4].Split('.')[0],
            LogLineSection.ClassName => lineSections[4].Split('.')[1],
            LogLineSection.LogMessage => string.Join(' ', lineSections.Skip(5)),
            _ => throw new ArgumentException($"Invalid section name: {logLineSection}")
        };
    }
}
