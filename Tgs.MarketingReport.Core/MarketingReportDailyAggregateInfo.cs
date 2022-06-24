namespace Tgs.MarketingReport.Core;

public record MarketingReportDailyAggregateInfo(
    Guid AgentId,
    string AgentUserName,
    DateOnly ReportDate,
    int RegisterCount,
    int FirstDepositCount,
    int GamingCount);
