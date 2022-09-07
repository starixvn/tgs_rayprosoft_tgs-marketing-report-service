using Tgs.MarketingReport.Core.Cache;
using Tgs.ServiceUtility;
using Tgs.MarketingReport.ApplicationServices;
using Tgs.MarketingReport;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMarketingReportCache(
        this IServiceCollection services)
        => services
            .AddSingleton<ISystemClock, DefaultSystemClock>()
            .Decorate<IQueryService<GetDailyAggregateInfosQuery, AgentMarketingStatistics>,
                GetMemoryCachedDailyAggregateInfosQueryService>();
}
