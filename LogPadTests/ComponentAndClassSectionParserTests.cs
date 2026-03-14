using LogPad.LogParserExtensions;
using LogParser;
using LogParser.SectionParsers;

namespace LogPadTests;

public class ComponentAndClassSectionParserTests
{
    [Theory]
    [InlineData(new string[] { "DateTime" }, "2024-01-01 12:00:00 OnlineStore/UserService")]
    [InlineData(new string[] { "DateTime" }, "2024-01-01 12:00:00 OnlineStore UserService")]
    [InlineData(new string[] { "DateTime" }, "2024-01-01 12:00:00 OnlineStore:UserService")]
    [InlineData(new string[] { "DateTime" }, "2024-01-01 12:00:00 OnlineStore")]
    [InlineData(new string[] { "DateTime" }, "2024-01-01 12:00:00 OnlineStore_UserService")]
    public void BuildLogLine_ShouldBeSuccessful_WhenLogLineFormatIsInTheExpextedFormat(string[] SectionParserTypes, string logLine)
    {
        ISectionParser componentAndClassSectionParser = new ComponentAndClassSectionParser();
        List<SectionParser> sectionParsers = new List<SectionParser>();
        foreach (string sectionsParserType in SectionParserTypes)
        {
            SectionParser sectionParser = Enum.Parse<SectionParser>(sectionsParserType);
            sectionParsers.Add(sectionParser);
        }

        var costumSectionParsers = new Dictionary<int, ISectionParser>
        {
            { 1, componentAndClassSectionParser }
        };

        LogLineFormat logLineFormat = new LogLineFormat(sectionParsers, costumSectionParsers);
        LogLineParserResult logLineResult = LogLineParser.BuildLogLine(logLine, logLineFormat);
        Assert.NotNull(logLineResult);
        Assert.False(logLineResult.Success);
    }

    [Theory]
    [InlineData(new string[] { "DateTime", "LogLevel", "LogMessage" }, "2024-01-01 12:00:00 INFO OnlineSore.UserService - This is a log line message", 2, new string[] { "2024-01-01 12:00:00", "INFO", "OnlineSore", "UserService", "This is a log line message" })]
    [InlineData(new string[] { "LogLevel", "DateTime", "LogMessage" }, "INFO 2024-01-01 12:00:00 OnlineSore.UserService - This is a log line message", 2, new string[] { "INFO", "2024-01-01 12:00:00", "OnlineSore", "UserService", "This is a log line message" })]
    [InlineData(new string[] { "LogLevel", "DateTime", "LogMessage" }, "OnlineSore.UserService - INFO 2024-01-01 12:00:00 This is a log line message", 0, new string[] { "OnlineSore", "UserService", "INFO", "2024-01-01 12:00:00", "This is a log line message" })]
    public void BuildLogLine_ShouldParseSectionsCorrectly_WhenLogLineFormatIsInTheExpextedFormat(string[] SectionParserTypes, string logLine, int index, string[] expectedSections)
    {
        ISectionParser componentAndClassSectionParser = new ComponentAndClassSectionParser();
        List<SectionParser> sectionParsers = new List<SectionParser>();
        foreach (string sectionsParserType in SectionParserTypes)
        {
            SectionParser sectionParser = Enum.Parse<SectionParser>(sectionsParserType);
            sectionParsers.Add(sectionParser);
        }

        var costumSectionParsers = new Dictionary<int, ISectionParser>
        {
            { index, componentAndClassSectionParser }
        };

        LogLineFormat logLineFormat = new LogLineFormat(sectionParsers, costumSectionParsers);
        LogLineParserResult logLineResult = LogLineParser.BuildLogLine(logLine, logLineFormat);
        Assert.NotNull(logLineResult);
        Assert.True(logLineResult.Success);

        for (int i = 0; i < expectedSections.Length; i++)
        {
            Assert.Equal(expectedSections[i], logLineResult.LogLineSections[i]);
        }
    }
}
