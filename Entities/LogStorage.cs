using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogPad.Entities;

public record LogStorage
{
    public List<LogLine> AllLogs { get; init; }
    public List<LogLine> FilteredLogs { get; init; }

}
