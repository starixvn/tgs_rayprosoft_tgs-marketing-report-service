using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Tgs.ServiceUtility;
using Tgs.MarketingReport.ApplicationServices;
using Tgs.MarketingReport;

namespace Tgs.MarketingReport.Core.Cache;

internal class GetMemoryCachedDailyAggregateInfosQueryService : IQueryService<GetDailyAggregateInfosQuery, AgentMarketingStatistics>
{
    private static readonly TimeSpan _Offset = TimeSpan.FromHours(8D);

    private readonly IMemoryCache _memoryCache;
    private readonly ScheduleOptions _scheduleOptions;
    private readonly IQueryService<GetDailyAggregateInfosQuery, AgentMarketingStatistics> _sourceService;
    private readonly ISystemClock _systemClock;

    public GetMemoryCachedDailyAggregateInfosQueryService(
        IOptions<ScheduleOptions> options,
        IQueryService<GetDailyAggregateInfosQuery, AgentMarketingStatistics> sourceService,
        IMemoryCache memoryCache,
        ISystemClock systemClock)
    {
        _scheduleOptions = options.Value;
        _sourceService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
    }

    public async IAsyncEnumerable<AgentMarketingStatistics> QueryAsync(GetDailyAggregateInfosQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var now = _systemClock.UtcNow.ToOffset(_Offset);

        // assume schedule process time is 1 hour
        var todayScheduleEndTime = new DateTimeOffset(now.Year, now.Month, now.Day, _scheduleOptions.ExecuteTime.Hour,
            _scheduleOptions.ExecuteTime.Minute, 0, _Offset).AddHours(1);

        DateOnly cacheDate;
        DateTimeOffset expiration;

        if (now > todayScheduleEndTime)
        {
            cacheDate = DateOnly.FromDateTime(now.DateTime);
            expiration = todayScheduleEndTime.AddDays(1);
        }
        else
        {
            cacheDate = DateOnly.FromDateTime(now.DateTime).AddDays(-1);
            expiration = todayScheduleEndTime;
        }

        var key = $"DailyAggregate:Brand:{query.BrandId}:{cacheDate}";

        var aggregateInfos = await _memoryCache.GetOrCreateAsync(key, async entry =>
        {
            var dataQuery = new GetDailyAggregateInfosQuery(
                query.BrandId,
                cacheDate.AddDays(-91),
                cacheDate);

            var infos = await _sourceService
                .QueryAsync(dataQuery, cancellationToken)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

            entry.AbsoluteExpiration = expiration;

            return infos;
        }).ConfigureAwait(false);

        foreach (var info in aggregateInfos
                     .Where(info => info.ReportDate >= query.StartDate &&
                                    info.ReportDate < query.EndDate))
        {
            yield return info;
        }
    }
}
