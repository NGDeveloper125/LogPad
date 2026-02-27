using System.Collections.Generic;

namespace LogPad.Entities;

public record LogStorage(List<LogLine> AllLogs, List<LogLine> FilteredLogs);
