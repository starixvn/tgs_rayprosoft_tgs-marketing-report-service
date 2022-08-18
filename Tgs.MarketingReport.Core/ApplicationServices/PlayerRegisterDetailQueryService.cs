using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.ApplicationServices;

internal class PlayerRegisterDetailQueryService : IQueryService<PlayerRegisterDetailQuery, PlayerRegisterItem>
{
    private readonly ISystemClock _systemClock;
    private readonly ITgsRepository _tgsRepository;

    public PlayerRegisterDetailQueryService(ITgsRepository tgsRepository, ISystemClock systemClock)
    {
        _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
        _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
    }

    public IAsyncEnumerable<PlayerRegisterItem> QueryAsync(PlayerRegisterDetailQuery query, CancellationToken cancellationToken = default)
    {
        var currentDate = DateOnly.FromDateTime(_systemClock.UtcNow.DateTime);

        return query.ReportDate >= currentDate
            ? _tgsRepository.QueryRealTimePlayerDailyRegisterStatisticsAsync(
                query.BrandId,
                query.ReportDate,
                cancellationToken)
            : _tgsRepository.QueryPlayerDailyRegisterStatisticsAsync(
                query.BrandId,
                query.ReportDate,
                cancellationToken);
    }
}
