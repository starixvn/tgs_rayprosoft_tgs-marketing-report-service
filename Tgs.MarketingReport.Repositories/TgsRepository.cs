using Tgs.MarketingReport.Repositories.Entities;
using Tgs.MarketingReport;

namespace Tgs.MarketingReport.Repositories;

internal class TgsRepository : ITgsRepository
{
    private readonly TgsContext _context;

    public TgsRepository(
        TgsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IAsyncEnumerable<AgentMarketingStatistics> GetDailyAggregateInfosAsync(
        Guid brandId,
        DateOnly startDate,
        DateOnly endDate)
        => (from aggregateInfo in _context.MarketingReportDailyAggregates
            where aggregateInfo.ReportDate >= startDate && aggregateInfo.ReportDate < endDate
            join agent in _context.Agents on aggregateInfo.AgentId equals agent.Id
            where agent.BrandId == brandId
            select new AgentMarketingStatistics(
                agent.Id,
                aggregateInfo.ReportDate,
                aggregateInfo.RegisterCount,
                aggregateInfo.GamingCount))
            .ToAsyncEnumerable();

    public IAsyncEnumerable<AgentMarketingStatistics> QueryAgentMarketingStatisticsAsync(
        Guid brandId,
        DateOnly queryLatestDate,
        CancellationToken cancellationToken = default)
    {
        var dateRange = (
            From: queryLatestDate.AddDays(-91),
            To: queryLatestDate);

        var query = _context.AgentDailyMarketingStatistics.AsQueryable()
            .Where(row => row.BrandId == brandId
                && row.ReportDate >= dateRange.From
                && row.ReportDate < dateRange.To)
            .Select(row => new AgentMarketingStatistics(
                row.AgentId,
                row.ReportDate,
                row.RegisterCount,
                row.GamingCount));

        return query
            .ToArray()
            .ToAsyncEnumerable();
    }
}
