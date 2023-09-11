using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.ApplicationServices;

internal class BrandMarketingQueryService : IGetService<BrandMarketingQuery, BrandMarketingStatistics?>
{
    private readonly ITgsRepository _tgsRepository;

    public BrandMarketingQueryService(ITgsRepository tgsRepository)
    {
        _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
    }

    public ValueTask<BrandMarketingStatistics?> GetAsync(BrandMarketingQuery query, CancellationToken cancellationToken = default)
        => _tgsRepository.QueryBrandMarketingStatisticAsync(query.BrandId, query.QueryDate, cancellationToken);
}
