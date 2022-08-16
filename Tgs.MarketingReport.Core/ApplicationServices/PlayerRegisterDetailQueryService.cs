using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.ApplicationServices;

internal class PlayerRegisterDetailQueryService : IQueryService<PlayerRegisterDetailQuery, PlayerRegisterItem>
{
    private readonly ITgsRepository _tgsRepository;

    public PlayerRegisterDetailQueryService(ITgsRepository tgsRepository)
    {
        _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
    }

    public IAsyncEnumerable<PlayerRegisterItem> QueryAsync(PlayerRegisterDetailQuery query, CancellationToken cancellationToken = default)
        => _tgsRepository.QueryPlayerDailyRegisterStatisticsAsync(
            query.BrandId,
            query.ReportDate,
            cancellationToken);
}
