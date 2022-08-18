// ReSharper disable CheckNamespace

using System.Reflection;
using Tgs.MarketingReport;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMarketingReportCore(
        this IServiceCollection services,
        Action<ScheduleOptions, IServiceProvider> configure)
        => services
            .AddOptions<ScheduleOptions>()
            .Configure(configure)
            .Services
            .RegisterAllServiceUtilityTypes(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient)
            .AddSingleton<ISystemClock, DefaultSystemClock>();
}
