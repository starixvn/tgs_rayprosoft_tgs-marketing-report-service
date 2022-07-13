using NSubstitute;
using Tgs.MarketingReport.Core.ApplicationServices;

namespace Tgs.MarketingReport.Core.UnitTests;

internal class GetDailyAggregateInfosQueryServiceUnitTests
{
    [Test]
    public async Task Can_Get_DailyAggregate_Infos()
    {
        var fakeRepository = Substitute.For<ITgsRepository>();

        var brandId = Guid.NewGuid();
        var startDate = DateOnly.Parse("2022-06-20");
        var endDate = DateOnly.Parse("2022-06-22");

        var query = new GetDailyAggregateInfosQuery(brandId, startDate, endDate);

        var sut = new GetDailyAggregateInfosQueryService(fakeRepository);

        _ = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(false);

        _ = fakeRepository
            .Received(1)
            .GetDailyAggregateInfosAsync(
                Arg.Is(query.BrandId),
                Arg.Is(query.StartDate),
                Arg.Is(query.EndDate));
    }
}
