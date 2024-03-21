using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.ServiceUtility;
using Tgs.MarketingReport.ApplicationServices;
using Tgs.MarketingReport;
using NUnit.Framework.Legacy;

namespace Tgs.MarketingReport.Core.Cache.UnitTests;

public class GetMemoryCachedDailyAggregateInfosQueryServiceUnitTests
{
    [Test]
    public async Task Get_Data_Hit_Cache()
    {
        var fakeScheduleOptions = new OptionsWrapper<ScheduleOptions>(new ScheduleOptions
        {
            ExecuteTime = TimeOnly.Parse("06:00")
        });

        var fakeMemoryCache = new MemoryCache(new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions()));

        var fakeQueryService = Substitute.For<IQueryService<GetDailyAggregateInfosQuery, AgentMarketingStatistics>>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var brandId = Guid.NewGuid();
        var now = DateTimeOffset.Parse("2022-07-01 08:00:00+08:00");

        _ = fakeSystemClock.UtcNow.Returns(now);

        var expected = new AgentMarketingStatistics[]
        {
            new(Guid.NewGuid(), DateOnly.Parse("2022-07-01"), 1, 0),
            new(Guid.NewGuid(), DateOnly.Parse("2022-07-01"), 0, 1),
        };

        var currentDate = DateOnly.Parse("2022-07-01");

        _ = fakeMemoryCache.Set($"DailyAggregate:Brand:{brandId}:{currentDate}", expected);

        var sut = new GetMemoryCachedDailyAggregateInfosQueryService(
            fakeScheduleOptions,
            fakeQueryService,
            fakeMemoryCache,
            fakeSystemClock);

        var query = new GetDailyAggregateInfosQuery(
            brandId,
            DateOnly.Parse("2022-06-30"),
            DateOnly.Parse("2022-07-02"));

        var actual = await sut.QueryAsync(query).ToArrayAsync();

        CollectionAssert.AreEqual(expected, actual);

        _ = fakeQueryService
                .DidNotReceive()
                .QueryAsync(
                    Arg.Any<GetDailyAggregateInfosQuery>(),
                    Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Get_Data_Miss_Cache()
    {
        var fakeScheduleOptions = new OptionsWrapper<ScheduleOptions>(new ScheduleOptions
        {
            ExecuteTime = TimeOnly.Parse("06:00")
        });

        var fakeMemoryCache = new MemoryCache(new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions()));

        var fakeQueryService = Substitute.For<IQueryService<GetDailyAggregateInfosQuery, AgentMarketingStatistics>>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var brandId = Guid.NewGuid();
        var now = DateTimeOffset.Parse("2022-07-01 08:00:00+08:00");

        _ = fakeSystemClock.UtcNow.Returns(now);

        var expected = new AgentMarketingStatistics[]
        {
            new(Guid.NewGuid(), DateOnly.Parse("2022-07-01"), 1, 0),
            new(Guid.NewGuid(), DateOnly.Parse("2022-07-01"), 0, 1),
        };

        var currentDate = DateOnly.Parse("2022-07-01");

        var dataQuery = new GetDailyAggregateInfosQuery(
            brandId,
            currentDate.AddDays(-91),
            currentDate);

        _ = fakeQueryService
            .QueryAsync(Arg.Is(dataQuery), Arg.Any<CancellationToken>())
            .Returns(expected.ToAsyncEnumerable());

        var sut = new GetMemoryCachedDailyAggregateInfosQueryService(
            fakeScheduleOptions,
            fakeQueryService,
            fakeMemoryCache,
            fakeSystemClock);

        var query = new GetDailyAggregateInfosQuery(
            brandId,
            DateOnly.Parse("2022-06-30"),
            DateOnly.Parse("2022-07-02"));

        var actual = await sut.QueryAsync(query).ToArrayAsync();

        CollectionAssert.AreEqual(expected, actual);

        _ = fakeQueryService
            .Received(1)
            .QueryAsync(
                Arg.Is(dataQuery),
                Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Get_Data_With_Previous_Day_Cache_After_Schedule_Time()
    {
        var fakeScheduleOptions = new OptionsWrapper<ScheduleOptions>(new ScheduleOptions
        {
            ExecuteTime = TimeOnly.Parse("06:00")
        });

        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeQueryService = Substitute.For<IQueryService<GetDailyAggregateInfosQuery, AgentMarketingStatistics>>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var brandId = Guid.NewGuid();
        var now = DateTimeOffset.Parse("2022-07-01 09:00:00+08:00");

        _ = fakeSystemClock.UtcNow.Returns(now);

        var cachedDate = DateOnly.Parse("2022-07-01");

        var sut = new GetMemoryCachedDailyAggregateInfosQueryService(
            fakeScheduleOptions,
            fakeQueryService,
            fakeMemoryCache,
            fakeSystemClock);

        var query = new GetDailyAggregateInfosQuery(
            brandId,
            DateOnly.Parse("2022-06-30"),
            DateOnly.Parse("2022-07-02"));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeMemoryCache
            .Received(1)
            .GetOrCreateAsync(
                Arg.Is($"DailyAggregate: Brand:{brandId}:{cachedDate}"),
                Arg.Any<Func<ICacheEntry, Task<AgentMarketingStatistics[]>>>());
    }

    [Test]
    public async Task Get_Data_With_Previous_Day_Cache_Before_Schedule_Time()
    {
        var fakeScheduleOptions = new OptionsWrapper<ScheduleOptions>(new ScheduleOptions
        {
            ExecuteTime = TimeOnly.Parse("06:00")
        });

        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeQueryService = Substitute.For<IQueryService<GetDailyAggregateInfosQuery, AgentMarketingStatistics>>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var brandId = Guid.NewGuid();
        var now = DateTimeOffset.Parse("2022-07-01 00:00:00+08:00");

        _ = fakeSystemClock.UtcNow.Returns(now);

        var cachedDate = DateOnly.Parse("2022-06-30");

        var sut = new GetMemoryCachedDailyAggregateInfosQueryService(
            fakeScheduleOptions,
            fakeQueryService,
            fakeMemoryCache,
            fakeSystemClock);

        var query = new GetDailyAggregateInfosQuery(
            brandId,
            DateOnly.Parse("2022-06-30"),
            DateOnly.Parse("2022-07-02"));

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeMemoryCache
            .Received(1)
            .GetOrCreateAsync(
                Arg.Is($"DailyAggregate: Brand:{brandId}:{cachedDate}"),
                Arg.Any<Func<ICacheEntry, Task<AgentMarketingStatistics[]>>>());
    }
}
