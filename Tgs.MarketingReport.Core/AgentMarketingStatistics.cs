namespace Tgs.MarketingReport;

public record AgentMarketingStatistics(
    Guid AgentId,
    DateOnly ReportDate,
    int RegisterCount,
    int GamingCount);
