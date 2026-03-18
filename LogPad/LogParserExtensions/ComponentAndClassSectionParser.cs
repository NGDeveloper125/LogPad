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

        var pattern = @"^([A-Za-z_][A-Za-z0-9]*)\.([A-Za-z_][A-Za-z0-9]*)";
        var match = Regex.Match(workingLine.TrimStart(), pattern);

        if (match.Success)
        {
            var component = match.Groups[1].Value;
            var className = match.Groups[2].Value;
            return new LogLineParserResult(true, [component, className], string.Empty);
        }

        // Check if there's a second identifier separated by invalid separators (space, /, :, or _)
        var invalidSeparators = @"^([A-Za-z_][A-Za-z0-9]*)[\s/:_]([A-Za-z_][A-Za-z0-9]*)";
        var invalidMatch = Regex.Match(workingLine.TrimStart(), invalidSeparators);

        if (invalidMatch.Success)
        {
            // Two identifiers found with invalid separator - this is invalid format
            return new LogLineParserResult(false, [], "Failed to parse component and class section in log line");
        }

        // Single word pattern - succeeds only if no invalid second identifier follows
        var singleWordPattern = @"^([A-Za-z_][A-Za-z0-9]*)";
        var singleMatch = Regex.Match(workingLine.TrimStart(), singleWordPattern);

        if (singleMatch.Success)
        {
            var component = singleMatch.Groups[1].Value;
            return new LogLineParserResult(true, [component, ""], string.Empty);
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

        // Remove any leading brackets that may have been consumed by previous parsers but not returned
        // This handles cases like "[108] INFO" where LogLevelSectionParser matched the pattern but only returned "INFO"
        remaining = Regex.Replace(remaining, @"^\s*\[[^\]]*\]\s*", "");

        return remaining;
    }
}
