namespace Tgs.MarketingReport;

public interface ITgsRepository
{
    IAsyncEnumerable<AgentMarketingStatistics> GetDailyAggregateInfosAsync(
        Guid brandId,
        DateOnly startDate,
        DateOnly endDate);

    IAsyncEnumerable<AgentMarketingStatistics> QueryAgentMarketingStatisticsAsync(
        Guid brandId,
        DateOnly queryLatestDate,
        CancellationToken cancellationToken = default);

    ValueTask<BrandMarketingStatistics?> QueryBrandMarketingStatisticAsync(
        Guid brandId,
        DateOnly queryDate,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<PlayerRegisterItem> QueryPlayerDailyRegisterStatisticsAsync(
        Guid brandId,
        DateOnly queryDate,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<PlayerGamingItem> QueryPlayerGamingStatisticsAsync(
        Guid brandId,
        DateOnly queryDate,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<PlayerGamingItem> QueryRealTimePlayerDailyGamingStatisticsAsync(
        Guid brandId,
        DateOnly queryDate,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<PlayerRegisterItem> QueryRealTimePlayerDailyRegisterStatisticsAsync(
            Guid brandId,
        DateOnly queryDate,
        CancellationToken cancellationToken = default);
}
