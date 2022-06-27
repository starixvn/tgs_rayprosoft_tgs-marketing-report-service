namespace Tgs.MarketingReport.Core.ApplicationServices;

public record GetMarketingReportDailyAggregateByAgentQuery(
    Guid BrandId,
    Guid AgentId,
    DateOnly StartDate,
    DateOnly EndDate);
