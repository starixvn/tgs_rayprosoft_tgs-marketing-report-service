using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.Core.ApplicationServices;

internal class GetMarketingReportDailyAggregateByAgentQueryService
    : IQueryService<GetMarketingReportDailyAggregateByAgentQuery, MarketingReportDailyAggregateInfo>
{
    private readonly IQueryService<GetMarketingReportDailyAggregateQuery, MarketingReportDailyAggregateInfo> _queryService;

    public GetMarketingReportDailyAggregateByAgentQueryService(
        IQueryService<GetMarketingReportDailyAggregateQuery, MarketingReportDailyAggregateInfo> queryService)
    {
        _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
    }

    public async IAsyncEnumerable<MarketingReportDailyAggregateInfo> QueryAsync(GetMarketingReportDailyAggregateByAgentQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var dataQuery = new GetMarketingReportDailyAggregateQuery(
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
