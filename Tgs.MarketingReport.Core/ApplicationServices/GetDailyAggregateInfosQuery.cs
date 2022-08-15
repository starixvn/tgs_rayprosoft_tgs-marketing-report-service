namespace Tgs.MarketingReport.ApplicationServices;

public record GetDailyAggregateInfosQuery(
    Guid BrandId,
    DateOnly StartDate,
    DateOnly EndDate);
