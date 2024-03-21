using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.ApplicationServices;

internal class BrandMarketingQueryService(ITgsRepository tgsRepository) : IGetService<BrandMarketingQuery, BrandMarketingStatistics?>
{
    private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

    public ValueTask<BrandMarketingStatistics?> GetAsync(BrandMarketingQuery query, CancellationToken cancellationToken = default)
        => _tgsRepository.QueryBrandMarketingStatisticAsync(query.BrandId, query.QueryDate, cancellationToken);
}
