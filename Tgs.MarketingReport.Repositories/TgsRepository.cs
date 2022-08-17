using Microsoft.EntityFrameworkCore;
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

    public IAsyncEnumerable<PlayerRegisterItem> QueryPlayerDailyRegisterStatisticsAsync(
        Guid brandId,
        DateOnly queryDate,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<PlayerRegisterItem>()
            .FromSqlInterpolated(@$"
                SELECT
                    S.player_code AS PlayerCode,
                    P.username AS PlayerName,
                    P.agent_id AS AgentId,
                    P.registered_on AS RegisteredOn,
                    S.last_login_on AS LastLoginOn,
                    S.daily_login_count AS LoginCount
                FROM
                    dbo.players AS P
                    JOIN
                    dbo.player_daily_register_statistics AS S ON P.player_code = S.player_code
                WHERE
                    P.brand_id = {brandId}
                    AND
                    S.report_date = {queryDate}
                    AND
                    P.agent_id IS NOT NULL");

        return query.ToAsyncEnumerable();
    }
}
