namespace Tgs.MarketingReport.Client;

public interface IMarketingReportClient
{
    public IAsyncEnumerable<MarketingReportDailyAggregateInfo> GetMarketingReportDailyAggregateInfosAsync(
        Guid brandId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);

    public IAsyncEnumerable<MarketingReportDailyAggregateInfo> GetMarketingReportDailyAggregateInfosAsync(
        Guid brandId,
        Guid agentId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);
}
