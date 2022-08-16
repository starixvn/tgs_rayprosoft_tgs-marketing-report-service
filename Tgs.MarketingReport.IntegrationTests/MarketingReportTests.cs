using EntityFrameworkCore.Testing.NSubstitute;
using EntityFrameworkCore.Testing.NSubstitute.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tgs.MarketingReport.Client;
using Tgs.MarketingReport.Repositories.Entities;

namespace Tgs.MarketingReport.IntegrationTests;

internal class MarketingReportTests
{
    [Test]
    public async Task Get_Marketing_Report_Daily_Aggregate_Infos_By_AgentId()
    {
        await using var web = new TestMarketingReportServiceApplication();

        var httpClient = web.CreateClient();

        var sut = new MarketingReportClient(httpClient);

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var agentId = Guid.NewGuid();
        var brandId = Guid.NewGuid();
        var agentUserName = "Username";

        var anotherAgentId = Guid.NewGuid();

        var expected = new[]
        {
            new DailyAggregateInfo
            {
                AgentId = agentId,
                GamingCount = 0,
                RegisterCount = 2,
                ReportDate = DateOnly.Parse("2022-06-22")
            },
            new DailyAggregateInfo
            {
                AgentId = agentId,
                GamingCount = 10,
                RegisterCount = 3,
                ReportDate = DateOnly.Parse("2022-06-23")
            }
        };

        await dbContext.Agents.AddRangeAsync(
            new Agent
            {
                Id = agentId,
                Username = agentUserName,
                AgentStatus = 0,
                BrandId = brandId,
                DefaultWalletId = Guid.NewGuid(),
                InvitationCode = "123456",
                Password = Array.Empty<byte>(),
                Salt = Array.Empty<byte>(),
                WithdrawPassword = Array.Empty<byte>(),
                WithdrawSalt = Array.Empty<byte>()
            },
            new Agent
            {
                Id = anotherAgentId,
                Username = anotherAgentId.ToString(),
                AgentStatus = 0,
                BrandId = brandId,
                DefaultWalletId = Guid.NewGuid(),
                InvitationCode = "123456",
                Password = Array.Empty<byte>(),
                Salt = Array.Empty<byte>(),
                WithdrawPassword = Array.Empty<byte>(),
                WithdrawSalt = Array.Empty<byte>()
            });

        await dbContext.MarketingReportDailyAggregates.AddRangeAsync(
            new MarketingReportDailyAggregate
            {
                AgentId = anotherAgentId,
                FirstDepositCount = 0,
                GamingCount = 5,
                RegisterCount = 0,
                ReportDate = DateOnly.Parse("2022-06-20")
            },
            new MarketingReportDailyAggregate
            {
                AgentId = anotherAgentId,
                FirstDepositCount = 1,
                GamingCount = 4,
                RegisterCount = 1,
                ReportDate = DateOnly.Parse("2022-06-21")
            },
            new MarketingReportDailyAggregate
            {
                AgentId = anotherAgentId,
                FirstDepositCount = 2,
                GamingCount = 8,
                RegisterCount = 0,
                ReportDate = DateOnly.Parse("2022-06-22")
            },
            new MarketingReportDailyAggregate
            {
                AgentId = agentId,
                FirstDepositCount = 0,
                GamingCount = 0,
                RegisterCount = 2,
                ReportDate = DateOnly.Parse("2022-06-22")
            },
            new MarketingReportDailyAggregate
            {
                AgentId = agentId,
                FirstDepositCount = 0,
                GamingCount = 10,
                RegisterCount = 3,
                ReportDate = DateOnly.Parse("2022-06-23")
            });

        _ = await dbContext.SaveChangesAsync();

        var startDate = DateOnly.Parse("2022-06-22");
        var endDate = DateOnly.Parse("2022-06-24");

        var actual = await sut
            .GetDailyAggregateInfosAsync(brandId, agentId, startDate, endDate)
            .ToArrayAsync();

        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public async Task Get_Marketing_Report_Daily_Aggregate_Infos_By_Brand()
    {
        await using var web = new TestMarketingReportServiceApplication();

        var httpClient = web.CreateClient();

        var sut = new MarketingReportClient(httpClient);

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var agentId = Guid.NewGuid();
        var brandId = Guid.NewGuid();
        var agentUserName = "Username";

        var expected = new[]
        {
            new DailyAggregateInfo
            {
                AgentId = agentId,
                GamingCount = 0,
                RegisterCount = 2,
                ReportDate = DateOnly.Parse("2022-06-22")
            },
            new DailyAggregateInfo
            {
                AgentId = agentId,
                GamingCount = 10,
                RegisterCount = 3,
                ReportDate = DateOnly.Parse("2022-06-23")
            }
        };

        _ = await dbContext.Agents.AddAsync(
            new Agent
            {
                Id = agentId,
                Username = agentUserName,
                AgentStatus = 0,
                BrandId = brandId,
                DefaultWalletId = Guid.NewGuid(),
                InvitationCode = "123456",
                Password = Array.Empty<byte>(),
                Salt = Array.Empty<byte>(),
                WithdrawPassword = Array.Empty<byte>(),
                WithdrawSalt = Array.Empty<byte>()
            });

        await dbContext.MarketingReportDailyAggregates.AddRangeAsync(
            new MarketingReportDailyAggregate
            {
                AgentId = agentId,
                FirstDepositCount = 0,
                GamingCount = 5,
                RegisterCount = 0,
                ReportDate = DateOnly.Parse("2022-06-20")
            },
            new MarketingReportDailyAggregate
            {
                AgentId = agentId,
                FirstDepositCount = 1,
                GamingCount = 4,
                RegisterCount = 1,
                ReportDate = DateOnly.Parse("2022-06-21")
            },
            new MarketingReportDailyAggregate
            {
                AgentId = agentId,
                FirstDepositCount = 0,
                GamingCount = 0,
                RegisterCount = 2,
                ReportDate = DateOnly.Parse("2022-06-22")
            },
            new MarketingReportDailyAggregate
            {
                AgentId = agentId,
                FirstDepositCount = 0,
                GamingCount = 10,
                RegisterCount = 3,
                ReportDate = DateOnly.Parse("2022-06-23")
            });

        _ = await dbContext.SaveChangesAsync();

        var startDate = DateOnly.Parse("2022-06-22");
        var endDate = DateOnly.Parse("2022-06-24");

        var actual = await sut
            .GetDailyAggregateInfosAsync(brandId, startDate, endDate)
            .ToArrayAsync();

        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public async Task Query_Marketing_Report_By_Query_Date()
    {
        await using var web = new TestMarketingReportServiceApplication();

        var httpClient = web.CreateClient();

        var sut = new MarketingReportClient(httpClient);

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var agentId = Guid.NewGuid();
        var brandId = Guid.NewGuid();

        var expected = new[]
        {
            new DailyAggregateInfo
            {
                AgentId = agentId,
                GamingCount = 0,
                RegisterCount = 2,
                ReportDate = DateOnly.Parse("2022-06-22")
            },
            new DailyAggregateInfo
            {
                AgentId = agentId,
                GamingCount = 10,
                RegisterCount = 3,
                ReportDate = DateOnly.Parse("2022-06-23")
            }
        };

        await dbContext.AgentDailyMarketingStatistics.AddRangeAsync(
            new AgentDailyMarketingStatistic
            {
                AgentId = agentId,
                BrandId = brandId,
                GamingCount = 0,
                RegisterCount = 2,
                ReportDate = DateOnly.Parse("2022-06-22")
            },
            new AgentDailyMarketingStatistic
            {
                AgentId = agentId,
                BrandId = brandId,
                GamingCount = 10,
                RegisterCount = 3,
                ReportDate = DateOnly.Parse("2022-06-23")
            });

        _ = await dbContext.SaveChangesAsync();

        var queryDate = DateOnly.Parse("2022-06-24");

        var actual = await sut
            .QueryMarketingStatisticsAsync(brandId, queryDate)
            .ToArrayAsync();

        CollectionAssert.AreEquivalent(expected, actual);
    }

    [Test]
    public async Task Query_Player_Register_Detail_By_Report_Date()
    {
        await using var web = new TestMarketingReportServiceApplication();

        var httpClient = web.CreateClient();

        var sut = new MarketingReportClient(httpClient);

        var brandId = Guid.NewGuid();
        var reportDate = DateOnly.Parse("2022-6-24");

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        dbContext.Set<PlayerRegisterItem>().AddFromSqlInterpolatedResult(
            new[] {
                new PlayerRegisterItem(
                    "test1",
                    "Test1",
                    Guid.NewGuid(),
                    DateTime.Now,
                    DateTime.Now,
                    1)
            });

        var actual = await sut
            .QueryPlayerRegisterDetailAsync(brandId, reportDate)
            .ToArrayAsync();

        Assert.AreEqual(1, actual.Length);
        Assert.AreEqual("test1", actual[0].PlayerCode);
    }
}
