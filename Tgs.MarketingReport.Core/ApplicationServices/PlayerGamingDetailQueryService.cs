using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.ApplicationServices;

internal class PlayerGamingDetailQueryService(ITgsRepository tgsRepository, ISystemClock systemClock) : IQueryService<PlayerGamingDetailQuery, PlayerGamingItem>
{
    private readonly ISystemClock _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
    private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

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
