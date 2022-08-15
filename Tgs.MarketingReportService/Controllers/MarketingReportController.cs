using Microsoft.AspNetCore.Mvc;
using Tgs.MarketingReportService.ViewModels;
using Tgs.ServiceUtility;
using Tgs.MarketingReport.ApplicationServices;
using Tgs.MarketingReport;

namespace Tgs.MarketingReportService.Controllers;

[Route("[controller]")]
[ApiController]
public class MarketingReportController : ControllerBase
{
    [HttpGet("{brandId}", Name = "GetMarketingReportDailyAggregatesWithBrandId")]
    public async IAsyncEnumerable<MarketingReportDailyAggregateViewModel> GetMarketingReportDailyAggregateDataAsync(
        [FromServices] IQueryService<GetDailyAggregateInfosQuery, AgentMarketingStatistics> queryService,
        Guid brandId,
        DateOnly startDate,
        DateOnly endDate)
    {
        var query = new GetDailyAggregateInfosQuery(brandId, startDate, endDate);

        await foreach (var info in queryService.QueryAsync(query, HttpContext.RequestAborted))
            yield return new MarketingReportDailyAggregateViewModel
            {
                AgentId = info.AgentId,
                GamingCount = info.GamingCount,
                RegisterCount = info.RegisterCount,
                ReportDate = info.ReportDate
            };
    }

    [HttpGet("{brandId}/{agentId}", Name = "GetMarketingReportDailyAggregatesWithAgentId")]
    public async IAsyncEnumerable<MarketingReportDailyAggregateViewModel> GetMarketingReportDailyAggregateDataAsync(
        [FromServices] IQueryService<GetDailyAggregateInfosByAgentQuery, AgentMarketingStatistics> queryService,
        Guid brandId,
        Guid agentId,
        DateOnly startDate,
        DateOnly endDate)
    {
        var query = new GetDailyAggregateInfosByAgentQuery(brandId, agentId, startDate, endDate);

        await foreach (var info in queryService.QueryAsync(query, HttpContext.RequestAborted))
            yield return new MarketingReportDailyAggregateViewModel
            {
                AgentId = info.AgentId,
                FirstDepositCount = 0,
                GamingCount = info.GamingCount,
                RegisterCount = info.RegisterCount,
                ReportDate = info.ReportDate
            };
    }
}
