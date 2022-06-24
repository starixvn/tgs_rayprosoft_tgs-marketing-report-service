using NSubstitute;
using Tgs.MarketingReport.Core.ApplicationServices;

namespace Tgs.MarketingReport.Core.UnitTests;

internal class GetMarketingReportDailyAggregateQueryServiceUnitTests
{
    [Test]
    public async Task Can_Get_MarketingReportDailyAggregate_Infos()
    {
        var fakeRepository = Substitute.For<ITgsRepository>();

        var brandId = Guid.NewGuid();
        var startDate = DateOnly.Parse("2022-06-20");
        var endDate = DateOnly.Parse("2022-06-22");

        var query = new GetMarketingReportDailyAggregateQuery(brandId, startDate, endDate);

        var sut = new GetMarketingReportDailyAggregateQueryService(fakeRepository);

        _ = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(false);

        _ = fakeRepository
            .Received(1)
            .GetMarketingReportDailyAggregateInfosAsync(
                Arg.Is(query.BrandId),
                Arg.Is(query.StartDate),
                Arg.Is(query.EndDate));
    }
}