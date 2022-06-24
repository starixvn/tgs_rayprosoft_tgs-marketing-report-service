using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.Core.ApplicationServices;

internal class GetMarketingReportDailyAggregateQueryService
    : IQueryService<GetMarketingReportDailyAggregateQuery, MarketingReportDailyAggregateInfo>
{
    private readonly ITgsRepository _tgsRepository;

    public GetMarketingReportDailyAggregateQueryService(
        ITgsRepository tgsRepository)
    {
        _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
    }

    public IAsyncEnumerable<MarketingReportDailyAggregateInfo> QueryAsync(
        GetMarketingReportDailyAggregateQuery query,
        CancellationToken cancellationToken = default)
        => _tgsRepository.GetMarketingReportDailyAggregateInfosAsync(query.BrandId, query.StartDate, query.EndDate);
}