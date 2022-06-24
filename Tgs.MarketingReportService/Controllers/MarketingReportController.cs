using Microsoft.AspNetCore.Mvc;
using Tgs.MarketingReport.Core;
using Tgs.MarketingReport.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.MarketingReportService.Controllers;

[Route("[controller]")]
[ApiController]
public class MarketingReportController : ControllerBase
{
    [HttpGet("brand/{brandId}")]
    public IAsyncEnumerable<MarketingReportDailyAggregateInfo> GetMarketingReportDailyAggregateDataAsync(
        [FromServices] IQueryService<GetMarketingReportDailyAggregateQuery, MarketingReportDailyAggregateInfo> queryService,
        Guid brandId,
        DateOnly startDate,
        DateOnly endDate)
    {
        var query = new GetMarketingReportDailyAggregateQuery(brandId, startDate, endDate);

        return queryService.QueryAsync(query, HttpContext.RequestAborted);
    }
}
