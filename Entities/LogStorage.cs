using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogPad.Entities;

public record LogStorage
{
    public List<LogRaw> AllLogs { get; init; }
    public List<LogRaw> FilteredLogs { get; init; }

}
