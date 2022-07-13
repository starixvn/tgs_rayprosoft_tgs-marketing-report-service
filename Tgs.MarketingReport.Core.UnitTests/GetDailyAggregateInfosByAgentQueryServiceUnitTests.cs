using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Tgs.MarketingReport.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.MarketingReport.Core.UnitTests;

internal class GetDailyAggregateInfosByAgentQueryServiceUnitTests
{
    [Test]
    public async Task Can_Get_DailyAggregate_Infos_By_AgentId()
    {
        var fakeQueryService = Substitute.For<IQueryService<GetDailyAggregateInfosQuery, DailyAggregateInfo>>();

        var brandId = Guid.NewGuid();
        var agentId = Guid.NewGuid();
        var startDate = DateOnly.Parse("2022-06-20");
        var endDate = DateOnly.Parse("2022-06-22");

        var query = new GetDailyAggregateInfosByAgentQuery(brandId, agentId, startDate, endDate);

        var sut = new GetDailyAggregateInfosByAgentQueryService(fakeQueryService);

        _ = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(false);

        _ = fakeQueryService
            .Received(1)
            .QueryAsync(
                Arg.Is<GetDailyAggregateInfosQuery>(x =>
                    x.BrandId == query.BrandId &&
                    x.StartDate == query.StartDate &&
                    x.EndDate == query.EndDate));
    }
}
