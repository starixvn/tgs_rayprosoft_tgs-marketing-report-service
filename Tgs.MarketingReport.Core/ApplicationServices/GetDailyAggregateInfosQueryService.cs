using Tgs.ServiceUtility;
using Tgs.MarketingReport.ApplicationServices;

namespace Tgs.MarketingReport.ApplicationServices;

internal class GetDailyAggregateInfosQueryService
    : IQueryService<GetDailyAggregateInfosQuery, AgentMarketingStatistics>
{
    private readonly ITgsRepository _tgsRepository;

    public GetDailyAggregateInfosQueryService(
        ITgsRepository tgsRepository)
    {
        _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
    }

    public IAsyncEnumerable<AgentMarketingStatistics> QueryAsync(
        GetDailyAggregateInfosQuery query,
        CancellationToken cancellationToken = default)
        => _tgsRepository.GetDailyAggregateInfosAsync(query.BrandId, query.StartDate, query.EndDate);
}
