namespace Tgs.MarketingReport.Core;

public record DailyAggregateInfo(
    Guid AgentId,
    string AgentUserName,
    DateOnly ReportDate,
    int RegisterCount,
    int FirstDepositCount,
    int GamingCount);
