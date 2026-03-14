using LogPad.Entities;
using LogPad.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                newLogLines.Add
                (
                    new LogLine
                    (
                        file.FileName, 
                        Guid.NewGuid(), 
                        line.GetTimeStamp(), 
                        line.GetLogLevel(), 
                        line.GetComponentName(), 
                        line.GetClassName(), 
                        line.GetLogMessage()
                    )
                );
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
