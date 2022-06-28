// ReSharper disable CheckNamespace

using Tgs.MarketingReport.Core;
using Tgs.MarketingReport.Core.ApplicationServices;
using Tgs.MarketingReport.Core.Cache;
using Tgs.ServiceUtility;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMarketingReportCache(this IServiceCollection services)
        => services
            .Decorate<IQueryService<GetDailyAggregateInfosQuery, DailyAggregateInfo>,
                GetCachedDailyAggregateInfosQueryService>();
}
