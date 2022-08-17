using NSubstitute;
using Tgs.MarketingReport.ApplicationServices;

namespace Tgs.MarketingReport.Core.UnitTests;

public class MarketingReportTests
{
    [Test]
    public async Task Query_Agent_MarketingReport()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new AgentMarketingQueryService(fakeTgsRepository);

        var query = new AgentMarketingQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryAgentMarketingStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.QueryDate));
    }

    [Test]
    public async Task Query_Player_Gaming_Detail()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new PlayerGamingDetailQueryService(fakeTgsRepository);

        var query = new PlayerGamingDetailQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryPlayerGamingStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.ReportDate));
    }

    [Test]
    public async Task Query_Player_Register_Detail()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new PlayerRegisterDetailQueryService(fakeTgsRepository);

        var query = new PlayerRegisterDetailQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryPlayerDailyRegisterStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.ReportDate));
    }
}
