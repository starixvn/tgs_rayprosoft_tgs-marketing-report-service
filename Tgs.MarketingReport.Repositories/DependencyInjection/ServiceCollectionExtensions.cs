// ReSharper disable CheckNamespace

using Microsoft.EntityFrameworkCore;
using Tgs.MarketingReport.Repositories;
using Tgs.MarketingReport.Repositories.Entities;
using Tgs.MarketingReport;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMarketingReportRepository(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> dbConfigure)
        => services
            .AddDbContext<TgsContext>(dbConfigure)
            .AddTransient<ITgsRepository, TgsRepository>();
}