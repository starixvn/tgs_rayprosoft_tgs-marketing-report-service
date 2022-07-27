namespace Tgs.MarketingReport.Core;

public record DailyAggregateInfo(
    Guid AgentId,
    DateOnly ReportDate,
    int RegisterCount,
    int FirstDepositCount,
    int GamingCount);
