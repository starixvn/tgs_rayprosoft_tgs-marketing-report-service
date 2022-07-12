using Microsoft.EntityFrameworkCore.Design;
using Tgs.MarketingReport.Core;
using Tgs.MarketingReport.Repositories.Entities;

namespace Tgs.MarketingReport.Repositories;

internal class TgsRepository : ITgsRepository
{
    private readonly TgsContext _context;

    public TgsRepository(
        TgsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IAsyncEnumerable<DailyAggregateInfo> GetDailyAggregateInfosAsync(
        Guid brandId,
        DateOnly startDate,
        DateOnly endDate)
        => (from aggregateInfo in _context.MarketingReportDailyAggregates
            where aggregateInfo.ReportDate >= startDate && aggregateInfo.ReportDate < endDate
            join agent in _context.Agents on aggregateInfo.AgentId equals agent.Id
            where agent.BrandId == brandId
            select new DailyAggregateInfo(
                agent.Id,
                aggregateInfo.ReportDate,
                aggregateInfo.RegisterCount,
                aggregateInfo.FirstDepositCount,
                aggregateInfo.GamingCount))
            .ToAsyncEnumerable();
}
