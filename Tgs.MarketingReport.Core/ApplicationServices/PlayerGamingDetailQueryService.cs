using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.ApplicationServices;

internal class PlayerGamingDetailQueryService : IQueryService<PlayerGamingDetailQuery, PlayerGamingItem>
{
    private readonly ISystemClock _systemClock;
    private readonly ITgsRepository _tgsRepository;

    public PlayerGamingDetailQueryService(ITgsRepository tgsRepository, ISystemClock systemClock)
    {
        _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
        _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
    }

    public IAsyncEnumerable<PlayerGamingItem> QueryAsync(PlayerGamingDetailQuery query, CancellationToken cancellationToken = default)
    {
        var currentDate = DateOnly.FromDateTime(_systemClock.UtcNow.DateTime);

        return query.ReportDate >= currentDate
            ? _tgsRepository.QueryRealTimePlayerDailyGamingStatisticsAsync(
                query.BrandId,
                query.ReportDate,
                cancellationToken)
            : _tgsRepository.QueryPlayerGamingStatisticsAsync(
                query.BrandId,
                query.ReportDate,
                cancellationToken);
    }
}
