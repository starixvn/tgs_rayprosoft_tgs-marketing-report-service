using Microsoft.AspNetCore.Mvc;
using Tgs.MarketingReport;
using Tgs.MarketingReport.ApplicationServices;
using Tgs.MarketingReportService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.MarketingReportService.Controllers;

[Route("[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    [HttpGet("marketing/gaming/{brandId}/{reportDate}")]
    public IAsyncEnumerable<PlayerGamingDetailViewModel> GetPlayerDailyGamingDetailAsync(
        [FromServices] IQueryService<PlayerGamingDetailQuery, PlayerGamingItem> queryService,
        Guid brandId,
        DateOnly reportDate)
    {
        var query = new PlayerGamingDetailQuery(brandId, reportDate);

        return queryService.QueryAsync(query, HttpContext.RequestAborted)
            .Select(item => new PlayerGamingDetailViewModel
            {
                PlayerCode = item.PlayerCode,
                PlayerName = item.PlayerName,
                AgentId = item.AgentId,
                GpCode = item.GpCode,
                SubGpCode = item.SubGpCode,
                WinLossAmount = item.WinLossAmount
            });
    }

    [HttpGet("marketing/register/{brandId}/{reportDate}")]
    public IAsyncEnumerable<PlayerRegisterDetailViewModel> GetPlayerDailyRegisterDetailAsync(
        [FromServices] IQueryService<PlayerRegisterDetailQuery, PlayerRegisterItem> queryService,
        Guid brandId,
        DateOnly reportDate)
    {
        var query = new PlayerRegisterDetailQuery(brandId, reportDate);

        return queryService.QueryAsync(query, HttpContext.RequestAborted)
            .Select(item => new PlayerRegisterDetailViewModel
            {
                PlayerCode = item.PlayerCode,
                PlayerName = item.PlayerName,
                AgentId = item.AgentId,
                RegisteredOn = item.RegisteredOn,
                LastLoginOn = item.LastLoginOn,
                LoginCount = item.LoginCount
            });
    }

    [HttpGet("marketing/{brandId}/{queryDate}", Name = "QueryMarketingStatisticsData")]
    public IAsyncEnumerable<MarketingReportDailyAggregateViewModel> QueryMarketingStatisticsDataAsync(
        [FromServices] IQueryService<AgentMarketingQuery, AgentMarketingStatistics> queryService,
        Guid brandId,
        DateOnly queryDate)
    {
        var query = new AgentMarketingQuery(brandId, queryDate);

        return queryService.QueryAsync(query, HttpContext.RequestAborted)
            .Select(s => new MarketingReportDailyAggregateViewModel
            {
                AgentId = s.AgentId,
                GamingCount = s.GamingCount,
                RegisterCount = s.RegisterCount,
                ReportDate = s.ReportDate
            });
    }
}
