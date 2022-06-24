namespace Tgs.MarketingReport.Client;

public interface IMarketingReportClient
{
    public IAsyncEnumerable<MarketingReportDailyAggregateInfo> GetMarketingReportDailyAggregateInfosAsync(
        Guid brandId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);
}
