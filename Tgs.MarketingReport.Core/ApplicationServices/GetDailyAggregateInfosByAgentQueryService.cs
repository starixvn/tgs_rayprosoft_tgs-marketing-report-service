using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;
using Tgs.MarketingReport.ApplicationServices;

namespace Tgs.MarketingReport.ApplicationServices;

internal class GetDailyAggregateInfosByAgentQueryService(
    IQueryService<GetDailyAggregateInfosQuery, AgentMarketingStatistics> queryService)
        : IQueryService<GetDailyAggregateInfosByAgentQuery, AgentMarketingStatistics>
{
    private readonly IQueryService<GetDailyAggregateInfosQuery, AgentMarketingStatistics> _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));

    public async IAsyncEnumerable<AgentMarketingStatistics> QueryAsync(GetDailyAggregateInfosByAgentQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var dataQuery = new GetDailyAggregateInfosQuery(
            query.BrandId,
            query.StartDate,
            query.EndDate);

        var aggregateInfos = await _queryService
            .QueryAsync(dataQuery, cancellationToken)
            .ToArrayAsync(cancellationToken);

        foreach (var aggregateInfo in aggregateInfos.Where(info => info.AgentId == query.AgentId))
            yield return aggregateInfo;
    }
}
