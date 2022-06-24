namespace Tgs.MarketingReport.Core;

public interface ITgsRepository
{
    IAsyncEnumerable<MarketingReportDailyAggregateInfo> GetMarketingReportDailyAggregateInfosAsync(
        Guid brandId,
        DateOnly startDate,
        DateOnly endDate);
}