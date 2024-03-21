using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.ApplicationServices;

internal class AgentMarketingQueryService(ITgsRepository tgsRepository) : IQueryService<AgentMarketingQuery, AgentMarketingStatistics>
{
    private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

    public IAsyncEnumerable<AgentMarketingStatistics> QueryAsync(AgentMarketingQuery query, CancellationToken cancellationToken = default)
        => _tgsRepository.QueryAgentMarketingStatisticsAsync(
            query.BrandId,
            query.QueryDate,
            cancellationToken);
}
