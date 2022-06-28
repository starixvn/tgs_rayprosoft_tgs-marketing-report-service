namespace Tgs.MarketingReport.Core.ApplicationServices;

public record GetDailyAggregateInfosQuery(
    Guid BrandId,
    DateOnly StartDate,
    DateOnly EndDate);
