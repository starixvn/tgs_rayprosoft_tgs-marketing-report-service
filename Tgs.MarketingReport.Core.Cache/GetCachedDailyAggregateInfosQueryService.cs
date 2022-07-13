using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Tgs.MarketingReport.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.Core.Cache;

internal class GetCachedDailyAggregateInfosQueryService : IQueryService<GetDailyAggregateInfosQuery, DailyAggregateInfo>
{
    private static readonly TimeSpan _EightHourTimeSpan = TimeSpan.FromHours(8D);

    private readonly IMemoryCache _memoryCache;
    private readonly ScheduleOptions _scheduleOptions;
    private readonly IQueryService<GetDailyAggregateInfosQuery, DailyAggregateInfo> _sourceService;

    public GetCachedDailyAggregateInfosQueryService(
        IOptions<ScheduleOptions> options,
        IQueryService<GetDailyAggregateInfosQuery, DailyAggregateInfo> sourceService,
        IMemoryCache memoryCache)
    {
        _scheduleOptions = options.Value;
        _sourceService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public async IAsyncEnumerable<DailyAggregateInfo> QueryAsync(GetDailyAggregateInfosQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var key = $"DailyAggregate:Brand:{query.BrandId}:{query.StartDate}:{query.EndDate}";

        var aggregateInfos = await _memoryCache.GetOrCreateAsync(key, async entry =>
        {
            var infos = await _sourceService
                .QueryAsync(query, cancellationToken)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

            var expiration = GetExpiration(query.StartDate, query.EndDate);

            entry.AbsoluteExpiration = expiration;

            return infos;
        }).ConfigureAwait(false);

        foreach (var info in aggregateInfos)
            yield return info;
    }

    private DateTimeOffset GetExpiration(DateOnly startDate, DateOnly endDate)
    {
        var start = new DateTimeOffset(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, _EightHourTimeSpan);
        var end = new DateTimeOffset(endDate.Year, endDate.Month, endDate.Day, 0, 0, 0, _EightHourTimeSpan);

        var now = DateTimeOffset.UtcNow.ToOffset(_EightHourTimeSpan);
        var scheduleTime = new DateTimeOffset(
            now.Year,
            now.Month,
            now.Day,
            _scheduleOptions.ExecuteTime.Hour,
            _scheduleOptions.ExecuteTime.Minute,
            0,
            _EightHourTimeSpan)
            .AddDays(1);

        if (start > scheduleTime)
            return now.AddDays(1D);

        if (end >= scheduleTime)
            return scheduleTime;

        var diffSeconds = (scheduleTime - now).TotalSeconds;
        if (diffSeconds > 24 * 60 * 60)
            return now.AddDays(1D);

        return scheduleTime;
    }
}
