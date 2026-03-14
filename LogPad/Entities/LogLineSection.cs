using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogPad.Entities;

public enum LogLineSection
{
    TimeStamp,
    LogLevel,
    ComponentName,
    ClassName,
    LogMessage
}
