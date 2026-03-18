using LogPad.Entities;
using LogPad.Extensions;
using LogPad.LogParserExtensions;
using LogParser;
using LogParser.SectionParsers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogLevel = LogPad.Entities.LogLevel;

namespace LogPad.Services;

public class LogStorageService
{
    public static async Task<LogStorage> AddLogsToLogStorage(ILogger logger, LogFile file, LogStorage logStorage)
    {
        List<LogLine> newLogLines = [];
        foreach (string line in file.FileContent)
        {
            try
            {
                ISectionParser componentAndClassSectionParser = new ComponentAndClassSectionParser();
                List<SectionParser> sectionParsers = new List<SectionParser>()
                {
                    Enum.Parse<SectionParser>("DateTime"),
                    Enum.Parse<SectionParser>("LogLevel"),
                    Enum.Parse<SectionParser>("LogMessage")
                };

                var costumSectionParsers = new Dictionary<int, ISectionParser>
                {
                    { 2, componentAndClassSectionParser }
                };

                LogLineFormat logLineFormat = new LogLineFormat(sectionParsers, costumSectionParsers);
                LogLineParserResult logLineResult = LogLineParser.BuildLogLine(line, logLineFormat);

                if (!logLineResult.Success)
                { 
                    logger.LogWarning($"Failed to parse log line: {line} in file: {file.FileName} | error: {logLineResult.ErrorMessage}");
                    continue;
                }

                DateTime logLineDateTime = DateTime.Parse(logLineResult.LogLineSections[0]);
                LogLevel logLineLogLevel = logLineResult.LogLineSections[1].GetLogLevel();


                newLogLines.Add(new LogLine(file.FileName, 
                                            Guid.NewGuid(),
                                            logLineDateTime,
                                            logLineLogLevel, 
                                            logLineResult.LogLineSections[2], 
                                            logLineResult.LogLineSections[3],
                                            logLineResult.LogLineSections[4]));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to parse log line: {line} in file: {file.FileName} | error: {ex.Message}");
                continue;
            }
        }
        logStorage.AllLogs.AddRange(newLogLines);

        List<LogLine> updatedFillteredLogLines = await LogFillterService.UpdateFilteredLogs(logStorage.AllLogs, logStorage.FilteredLogs, file.FileName);
        return new LogStorage(logStorage.AllLogs, updatedFillteredLogLines);
    }

    public static async Task<LogStorage> RemoveLogsFromLogStorage(List<LogFile> filesToRemove, LogStorage logStorage)
    {
        List<string> fileNamesToRemove = filesToRemove.Select(file => file.FileName).ToList();
        List<LogLine> updatedAllLogs = logStorage.AllLogs.Where(logLine => !fileNamesToRemove.Contains(logLine.SourceFileName)).ToList();
        List<LogLine> updatedFillteredLogLines = await LogFillterService.UpdateFilteredLogs(updatedAllLogs, logStorage.FilteredLogs);
        return new LogStorage(updatedAllLogs, updatedFillteredLogLines);
    }
}
