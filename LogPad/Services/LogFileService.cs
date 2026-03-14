using LogPad.Entities;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogPad.Services;

public class LogFileService
{
    private readonly IJSRuntime jSRuntime;

    public LogFileService(IJSRuntime jSRuntime)
    {
        this.jSRuntime = jSRuntime;
    }

    public async Task<FileDialogResult> OpenFileDialogAsync()
    {
        return await jSRuntime.InvokeAsync<FileDialogResult>("showOpenFileDialog",
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<(List<LogFile>, List<LogFile>)> UpdateFiles(List<LogFile> logFiles)
    {
        List<LogFile> files = logFiles.Select(logFile => logFile).Where(logFile => logFile.ToKeep).ToList();
        List<LogFile> filesToRemove = logFiles.Select(logFile => logFile).Where(logFile => !logFile.ToKeep).ToList();
        return (filesToRemove, files);
    }
}
