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
}
