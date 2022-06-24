namespace Tgs.MarketingReport.Core.ApplicationServices;

public record GetMarketingReportDailyAggregateQuery(
    Guid BrandId,
    DateOnly StartDate,
    DateOnly EndDate);