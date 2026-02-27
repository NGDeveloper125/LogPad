using System.Collections.Generic;

namespace LogPad.Entities;

public class LogFile
{
    public string FileName;
    public List<string> FileContent;
    public bool ToKeep;
}
