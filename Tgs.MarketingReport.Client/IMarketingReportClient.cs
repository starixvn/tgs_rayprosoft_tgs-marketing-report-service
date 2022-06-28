namespace Tgs.MarketingReport.Client;

public interface IMarketingReportClient
{
    public IAsyncEnumerable<DailyAggregateInfo> GetDailyAggregateInfosAsync(
        Guid brandId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);

    public IAsyncEnumerable<DailyAggregateInfo> GetDailyAggregateInfosAsync(
        Guid brandId,
        Guid agentId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);
}
