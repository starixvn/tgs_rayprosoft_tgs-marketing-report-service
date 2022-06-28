// ReSharper disable CheckNamespace

using Microsoft.Extensions.Options;
using Tgs.MarketingReport.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMarketingReportClient(
        this IServiceCollection services,
        Action<MarketingReportClientOptions, IServiceProvider> configure)
        => services
            .AddOptions<MarketingReportClientOptions>()
            .Configure(configure)
            .Services
            .AddHttpClient<IMarketingReportClient, MarketingReportClient>()
            .ConfigureHttpClient((sp, httpClient) =>
            {
                var options = sp.GetRequiredService<IOptions<MarketingReportClientOptions>>().Value;
                httpClient.BaseAddress = options.BaseUrl;
            })
            .Services;
}
