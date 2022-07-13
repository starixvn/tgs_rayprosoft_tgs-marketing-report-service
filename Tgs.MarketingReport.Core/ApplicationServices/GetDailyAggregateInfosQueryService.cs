using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.Core.ApplicationServices;

internal class GetDailyAggregateInfosQueryService
    : IQueryService<GetDailyAggregateInfosQuery, DailyAggregateInfo>
{
    private readonly ITgsRepository _tgsRepository;

    public GetDailyAggregateInfosQueryService(
        ITgsRepository tgsRepository)
    {
        _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
    }

    public IAsyncEnumerable<DailyAggregateInfo> QueryAsync(
        GetDailyAggregateInfosQuery query,
        CancellationToken cancellationToken = default)
        => _tgsRepository.GetDailyAggregateInfosAsync(query.BrandId, query.StartDate, query.EndDate);
}
