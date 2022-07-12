using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Tgs.MarketingReport.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.Core.Cache;

internal class GetCachedDailyAggregateInfosQueryService : IQueryService<GetDailyAggregateInfosQuery, DailyAggregateInfo>
{
    private static readonly TimeSpan _Offset = TimeSpan.FromHours(8D);

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
        var currentDate = DateOnly.FromDateTime(DateTimeOffset.UtcNow.ToOffset(_Offset).DateTime);

        var key = $"DailyAggregate:Brand:{query.BrandId}:{currentDate}";

        var aggregateInfos = await _memoryCache.GetOrCreateAsync(key, async entry =>
        {
            var dataQuery = new GetDailyAggregateInfosQuery(
                query.BrandId,
                currentDate.AddDays(-91),
                currentDate);

            var infos = await _sourceService
                .QueryAsync(dataQuery, cancellationToken)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

            var expiration = GetExpiration();

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

    private DateTimeOffset GetExpiration()
    {
        var now = DateTimeOffset.UtcNow.ToOffset(_Offset);
        var todayScheduleTime = new DateTimeOffset(
            now.Year,
            now.Month,
            now.Day,
            _scheduleOptions.ExecuteTime.Hour,
            _scheduleOptions.ExecuteTime.Minute,
            0,
            _Offset);

        return now < todayScheduleTime ? todayScheduleTime : now.AddDays(1);
    }
}
