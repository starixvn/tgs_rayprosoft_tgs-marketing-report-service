using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.Core.ApplicationServices;

internal class GetDailyAggregateInfosByAgentQueryService
    : IQueryService<GetDailyAggregateInfosByAgentQuery, DailyAggregateInfo>
{
    private readonly IQueryService<GetDailyAggregateInfosQuery, DailyAggregateInfo> _queryService;

    public GetDailyAggregateInfosByAgentQueryService(
        IQueryService<GetDailyAggregateInfosQuery, DailyAggregateInfo> queryService)
    {
        _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
    }

    public async IAsyncEnumerable<DailyAggregateInfo> QueryAsync(GetDailyAggregateInfosByAgentQuery query,
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
