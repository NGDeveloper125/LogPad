using LogParser;
using LogParser.SectionParsers;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LogPad.LogParserExtensions;

public class ComponentAndClassSectionParser : ISectionParser
{
    public LogLineParserResult ParseSection(string logLine, int sectionIndex, List<string> parsedSections)
    {
        if (string.IsNullOrEmpty(logLine))
            return new LogLineParserResult(false, [], "Failed to parse component and class section in log line");

        var workingLine = RemoveParsedSections(logLine, parsedSections);

        var pattern = @"([A-Za-z_][A-Za-z0-9_]*)\.([A-Za-z_][A-Za-z0-9_]*)";
        var match = Regex.Match(workingLine.TrimStart(), pattern);

        if (match.Success)
        {
            var component = match.Groups[1].Value;
            var className = match.Groups[2].Value;
            return new LogLineParserResult(true, [component, className], string.Empty);
        }

        return new LogLineParserResult(false, [], "Failed to parse component and class section in log line");
    }

    private string RemoveParsedSections(string logLine, List<string> parsedSections)
    {
        var remaining = logLine;
        foreach (var parsed in parsedSections)
        {
            if (!string.IsNullOrEmpty(parsed))
            {
                var index = remaining.IndexOf(parsed);
                if (index >= 0)
                {
                    remaining = remaining.Substring(index + parsed.Length);
                }
            }
        }
        return remaining;
    }
}
