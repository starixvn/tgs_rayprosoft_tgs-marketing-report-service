using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.ApplicationServices;

internal class PlayerGamingDetailQueryService : IQueryService<PlayerGamingDetailQuery, PlayerGamingItem>
{
    private readonly ITgsRepository _tgsRepository;

    public PlayerGamingDetailQueryService(ITgsRepository tgsRepository)
    {
        _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
    }

    public IAsyncEnumerable<PlayerGamingItem> QueryAsync(PlayerGamingDetailQuery query, CancellationToken cancellationToken = default)
        => _tgsRepository.QueryPlayerGamingStatisticsAsync(query.BrandId, query.ReportDate, cancellationToken);
}
