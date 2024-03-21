using EntityFrameworkCore.Testing.NSubstitute;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using Tgs.MarketingReport.Repositories.Entities;

namespace Tgs.MarketingReport.IntegrationTests;

extern alias MarketingReportSvc;

internal class TestMarketingReportServiceApplication(Action<IWebHostBuilder>? webHostConfigure = null) : WebApplicationFactory<MarketingReportSvc.Program>
{
    private readonly Action<IWebHostBuilder>? _webHostConfigure = webHostConfigure;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder
            .ConfigureAppConfiguration(c => c.Sources.Clear())
            .ConfigureServices(services => services
                .AddSingleton<IConfigureOptions<OtlpExporterOptions>>(
                    sp => new ConfigureNamedOptions<OtlpExporterOptions>(
                        Options.DefaultName,
                        exporterOptions =>
                        {
                            exporterOptions.TimeoutMilliseconds = 0;
                        }))
                .Replace(ServiceDescriptor.Singleton(sp =>
                {
                    var dbContextOptionsBuilder = new DbContextOptionsBuilder<TgsContext>();
                    _ = dbContextOptionsBuilder
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .ConfigureWarnings(x => x.Ignore(
                            InMemoryEventId.TransactionIgnoredWarning));

                    return Create.MockedDbContextFor<TgsContext>(dbContextOptionsBuilder.Options);
                })));

        _webHostConfigure?.Invoke(builder);
    }
}
