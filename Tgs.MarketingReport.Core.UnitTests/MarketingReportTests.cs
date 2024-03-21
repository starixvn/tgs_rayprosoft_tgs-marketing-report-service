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
    public async Task Query_Brand_MarketingReport()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new BrandMarketingQueryService(fakeTgsRepository);

        var query = new BrandMarketingQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = await sut.GetAsync(query);

        _ = fakeTgsRepository.Received(1)
            .QueryBrandMarketingStatisticAsync(Arg.Is(query.BrandId), Arg.Is(query.QueryDate));
    }

    [Test]
    public async Task Query_Player_Gaming_Detail()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new PlayerGamingDetailQueryService(fakeTgsRepository, new DefaultSystemClock());

        var query = new PlayerGamingDetailQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryPlayerGamingStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.ReportDate));
    }

    [Test]
    public async Task Query_Player_Gaming_Detail_By_Yestoday_On_0759_TimeZone8_Should_Use_RealTime()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var sut = new PlayerGamingDetailQueryService(fakeTgsRepository, fakeSystemClock);

        var query = new PlayerGamingDetailQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = fakeSystemClock.UtcNow.Returns(
            new DateTimeOffset(
                query.ReportDate.AddDays(1).ToDateTime(TimeOnly.Parse("07:59:00"), DateTimeKind.Unspecified),
                TimeSpan.FromHours(8)).ToOffset(TimeSpan.Zero));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryRealTimePlayerDailyGamingStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.ReportDate));
    }

    [Test]
    public async Task Query_Player_Gaming_Detail_By_Yestoday_On_0800_TimeZone8_Should_Use_Statistics()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var sut = new PlayerGamingDetailQueryService(fakeTgsRepository, fakeSystemClock);

        var query = new PlayerGamingDetailQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = fakeSystemClock.UtcNow.Returns(
            new DateTimeOffset(
                query.ReportDate.AddDays(1).ToDateTime(TimeOnly.Parse("08:00:00"), DateTimeKind.Unspecified),
                TimeSpan.FromHours(8)).ToOffset(TimeSpan.Zero));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryPlayerGamingStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.ReportDate));
    }

    [Test]
    public async Task Query_Player_Gaming_Detail_On_0000_TimeZone8_Should_Use_RealTime()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var sut = new PlayerGamingDetailQueryService(fakeTgsRepository, fakeSystemClock);

        var query = new PlayerGamingDetailQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = fakeSystemClock.UtcNow.Returns(
            new DateTimeOffset(
                query.ReportDate.ToDateTime(TimeOnly.Parse("00:00:00"), DateTimeKind.Unspecified),
                TimeSpan.FromHours(8)).ToOffset(TimeSpan.Zero));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryRealTimePlayerDailyGamingStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.ReportDate));
    }

    [Test]
    public async Task Query_Player_Gaming_Detail_On_0800_TimeZone8_Should_Use_RealTime()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var sut = new PlayerGamingDetailQueryService(fakeTgsRepository, fakeSystemClock);

        var query = new PlayerGamingDetailQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = fakeSystemClock.UtcNow.Returns(
            new DateTimeOffset(
                query.ReportDate.ToDateTime(TimeOnly.Parse("08:00:00"), DateTimeKind.Unspecified),
                TimeSpan.FromHours(8)).ToOffset(TimeSpan.Zero));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryRealTimePlayerDailyGamingStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.ReportDate));
    }

    [Test]
    public async Task Query_Player_Register_Detail()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new PlayerRegisterDetailQueryService(fakeTgsRepository, new DefaultSystemClock());

        var query = new PlayerRegisterDetailQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryPlayerDailyRegisterStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.ReportDate));
    }

    [Test]
    public async Task Query_Player_Register_Detail_By_Yestoday_On_0759_TimeZone8_Should_Use_RealTime()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var sut = new PlayerRegisterDetailQueryService(fakeTgsRepository, fakeSystemClock);

        var query = new PlayerRegisterDetailQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = fakeSystemClock.UtcNow.Returns(
            new DateTimeOffset(
                query.ReportDate.AddDays(1).ToDateTime(TimeOnly.Parse("07:59:00"), DateTimeKind.Unspecified),
                TimeSpan.FromHours(8)).ToOffset(TimeSpan.Zero));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryRealTimePlayerDailyRegisterStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.ReportDate));
    }

    [Test]
    public async Task Query_Player_Register_Detail_By_Yestoday_On_0800_TimeZone8_Should_Use_Statistics()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var sut = new PlayerRegisterDetailQueryService(fakeTgsRepository, fakeSystemClock);

        var query = new PlayerRegisterDetailQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = fakeSystemClock.UtcNow.Returns(
            new DateTimeOffset(
                query.ReportDate.AddDays(1).ToDateTime(TimeOnly.Parse("08:00:00"), DateTimeKind.Unspecified),
                TimeSpan.FromHours(8)).ToOffset(TimeSpan.Zero));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryPlayerDailyRegisterStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.ReportDate));
    }

    [Test]
    public async Task Query_Player_Register_Detail_On_0000_TimeZone8_Should_Use_RealTime()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var sut = new PlayerRegisterDetailQueryService(fakeTgsRepository, fakeSystemClock);

        var query = new PlayerRegisterDetailQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = fakeSystemClock.UtcNow.Returns(
            new DateTimeOffset(
                query.ReportDate.ToDateTime(TimeOnly.Parse("00:00:00"), DateTimeKind.Unspecified),
                TimeSpan.FromHours(8)).ToOffset(TimeSpan.Zero));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryRealTimePlayerDailyRegisterStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.ReportDate));
    }

    [Test]
    public async Task Query_Player_Register_Detail_On_0800_TimeZone8_Should_Use_RealTime()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var sut = new PlayerRegisterDetailQueryService(fakeTgsRepository, fakeSystemClock);

        var query = new PlayerRegisterDetailQuery(Guid.NewGuid(), DateOnly.Parse("2022-8-15"));

        _ = fakeSystemClock.UtcNow.Returns(
            new DateTimeOffset(
                query.ReportDate.ToDateTime(TimeOnly.Parse("08:00:00"), DateTimeKind.Unspecified),
                TimeSpan.FromHours(8)).ToOffset(TimeSpan.Zero));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryRealTimePlayerDailyRegisterStatisticsAsync(Arg.Is(query.BrandId), Arg.Is(query.ReportDate));
    }
}
