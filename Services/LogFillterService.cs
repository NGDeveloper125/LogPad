using LogPad.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogPad.Services;

public class LogFillterService
{

    public static async Task<List<LogLine>> UpdateFilteredLogs(List<LogLine>  allLogs, List<LogLine> filteredLogs, string fileName)
    {
        return [];
    }

    internal static async Task<List<LogLine>> UpdateFilteredLogs(List<LogLine> updatedAllLogs, List<LogLine> filteredLogs)
    {
        throw new NotImplementedException();
    }
}
