namespace Tgs.MarketingReport.ApplicationServices;

public record GetDailyAggregateInfosByAgentQuery(
    Guid BrandId,
    Guid AgentId,
    DateOnly StartDate,
    DateOnly EndDate);
