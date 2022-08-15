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
