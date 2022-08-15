using NSubstitute;
using Tgs.MarketingReport.ApplicationServices;

namespace Tgs.MarketingReport.Core.UnitTests;

public class MarketingReportTests
{
    [Test]
    public async Task Query_Agent_MarketingReportAsync()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new AgentMarketingQueryService(fakeTgsRepository);

        var query = new AgentMarketingQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryAgentMarketingStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.QueryDate));
    }
}
