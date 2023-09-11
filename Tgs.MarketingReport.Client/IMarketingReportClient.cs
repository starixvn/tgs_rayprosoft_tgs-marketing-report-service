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

    public ValueTask<DailyBrandMarketingStatisticInfo?> QueryBrandMarketingStatisticsAsync(Guid brandId, DateOnly queryDate, CancellationToken cancellationToken = default);

    public IAsyncEnumerable<DailyAggregateInfo> QueryMarketingStatisticsAsync(Guid brandId, DateOnly queryDate, CancellationToken cancellationToken = default);

    public IAsyncEnumerable<PlayerGamingDetail> QueryPlayerGamingDetailAsync(Guid brandId, DateOnly reportDate, CancellationToken cancellationToken = default);

    public IAsyncEnumerable<PlayerRegisterDetail> QueryPlayerRegisterDetailAsync(Guid brandId, DateOnly reportDate, CancellationToken cancellationToken = default);
}
