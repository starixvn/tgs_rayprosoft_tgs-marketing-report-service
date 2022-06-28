using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Tgs.MarketingReport.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.Core.Cache;

internal class GetCachedDailyAggregateInfosQueryService : IQueryService<GetDailyAggregateInfosQuery, DailyAggregateInfo>
{
    private readonly IMemoryCache _memoryCache;
    private readonly IQueryService<GetDailyAggregateInfosQuery, DailyAggregateInfo> _sourceService;

    public GetCachedDailyAggregateInfosQueryService(
        IQueryService<GetDailyAggregateInfosQuery, DailyAggregateInfo> sourceService,
        IMemoryCache memoryCache)
    {
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

            entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(1).ToOffset(TimeSpan.FromHours(8));

            return infos;
        }).ConfigureAwait(false);

        foreach (var info in aggregateInfos)
            yield return info;
    }
}
