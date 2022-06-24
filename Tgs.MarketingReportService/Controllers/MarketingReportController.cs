using Microsoft.AspNetCore.Mvc;
using Tgs.MarketingReport.Core;
using Tgs.MarketingReport.Core.ApplicationServices;
using Tgs.MarketingReportService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.MarketingReportService.Controllers;

[Route("[controller]")]
[ApiController]
public class MarketingReportController : ControllerBase
{
    [HttpGet("brand/{brandId}")]
    public async IAsyncEnumerable<MarketingReportDailyAggregateViewModel> GetMarketingReportDailyAggregateDataAsync(
        [FromServices] IQueryService<GetMarketingReportDailyAggregateQuery, MarketingReportDailyAggregateInfo> queryService,
        Guid brandId,
        DateOnly startDate,
        DateOnly endDate)
    {
        var query = new GetMarketingReportDailyAggregateQuery(brandId, startDate, endDate);

        await foreach (var info in queryService.QueryAsync(query, HttpContext.RequestAborted))
            yield return new MarketingReportDailyAggregateViewModel
            {
                AgentId = info.AgentId,
                AgentUserName = info.AgentUserName,
                FirstDepositCount = info.FirstDepositCount,
                GamingCount = info.GamingCount,
                RegisterCount = info.RegisterCount,
                ReportDate = info.ReportDate
            };
    }
}
