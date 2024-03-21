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

    public async ValueTask<BrandMarketingStatistics?> QueryBrandMarketingStatisticAsync(Guid brandId, DateOnly queryDate, CancellationToken cancellationToken = default)
    {    
        var query = _context.BrandDailyMarketingStatistics.AsQueryable()
            .Where(row => row.BrandId == brandId
                && row.ReportDate == queryDate)
            .Select(row => new BrandMarketingStatistics(
                row.BrandId,
                row.ReportDate,
                row.DirectlyPlayerGamingCount ?? 0,
                row.DirectlyPlayerRegisterCount ?? 0,
                row.TotalPlayerRegisterCount ?? 0,
                row.TotalPlayerGamingCount ?? 0));

        return await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
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

    public IAsyncEnumerable<PlayerGamingItem> QueryPlayerGamingStatisticsAsync(
        Guid brandId,
        DateOnly queryDate,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<PlayerGamingItem>()
            .FromSqlInterpolated(@$"
                SELECT
                    S.player_code AS PlayerCode,
                    P.username AS PlayerName,
                    P.agent_id AS AgentId,
                    S.gp_code AS GpCode,
                    S.sub_gp_code AS SubGpCode,
                    S.win_loss AS WinLossAmount
                FROM
                    dbo.players AS P
                    JOIN
                    dbo.player_daily_gaming_statistics AS S ON P.player_code = S.player_code
                WHERE
                    P.brand_id = {brandId}
                    AND
                    S.report_date = {queryDate}
                    AND
                    P.agent_id IS NOT NULL");

        return query.ToAsyncEnumerable();
    }

    public IAsyncEnumerable<PlayerGamingItem> QueryRealTimePlayerDailyGamingStatisticsAsync(
        Guid brandId,
        DateOnly queryDate,
        CancellationToken cancellationToken = default)
    {
        var reportDateTime = new DateTimeOffset(
            queryDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified),
            TimeSpan.FromHours(8D)).ToOffset(TimeSpan.Zero);

        var query = _context.Set<PlayerGamingItem>()
            .FromSqlInterpolated($@"
                SELECT
                    PGA.player_code AS PlayerCode,
                    P.username AS PlayerName,
                    PGA.agent_id AS AgentId,
                    GP.code AS GpCode,
                    G.sub_game_provider_code AS SubGpCode,
                    Sum(PGA.win_loss) AS WinLossAmount
                FROM
                    report.player_game_rebate_aggregates AS PGA
                    JOIN
                    dbo.game_providers AS GP ON PGA.game_provider_id = GP.id
                    JOIN
                    dbo.games AS G ON PGA.game_provider_id = G.game_provider_id
                        AND PGA.game_id = G.id
                    JOIN
                    dbo.players AS P ON PGA.player_code = P.player_code
                WHERE
                    PGA.brand_id = {brandId}
                    AND
                    PGA.report_date = {reportDateTime}
                    AND
                    PGA.agent_id IS NOT NULL
                GROUP BY
                    PGA.agent_id,
                    PGA.player_code,
                    P.username,
                    GP.code,
                    G.sub_game_provider_code");

        return query.ToAsyncEnumerable();
    }

    public IAsyncEnumerable<PlayerRegisterItem> QueryRealTimePlayerDailyRegisterStatisticsAsync(
        Guid brandId,
        DateOnly queryDate,
        CancellationToken cancellationToken = default)
    {
        var (From, To) = (queryDate, queryDate.AddDays(1));
        var queryTimeRange = (
            From: new DateTimeOffset(
                From
                    .AddDays(-1)
                    .ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.FromHours(16D)), DateTimeKind.Utc),
                TimeSpan.Zero),
            To: new DateTimeOffset(
                To
                    .AddDays(-1)
                    .ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.FromHours(16D)), DateTimeKind.Utc),
                TimeSpan.Zero));

        var query = _context.Set<PlayerRegisterItem>()
            .FromSqlInterpolated($@"
                SELECT
                    P.player_code AS PlayerCode,
                    P.username AS PlayerName,
                    P.agent_id AS AgentId,
                    P.registered_on AS RegisteredOn,
                    PIR.record_on AS LastLoginOn,
                    PLRC.login_count AS LoginCount
                FROM
                    dbo.players AS P
                    LEFT JOIN
                    dbo.player_ip_records AS PIR ON P.player_code = PIR.player_code
                        AND PIR.ip_type = 2
                    LEFT JOIN
                    (
                        SELECT
                            P1.player_code,
                            Count(1) AS login_count
                        FROM
                            dbo.players AS P1
                            JOIN
                            dbo.player_login_records AS PLR ON P1.player_code = PLR.player_code
                        WHERE
                            P1.brand_id = {brandId}
                            AND
                            P1.agent_id IS NOT NULL
                            AND
                            P1.registered_on >= {queryTimeRange.From}
                            AND
                            P1.registered_on < {queryTimeRange.To}
                        GROUP BY
                            P1.player_code
                    ) AS PLRC ON P.player_code = PLRC.player_code
                WHERE
                    P.brand_id = {brandId}
                    AND
                    P.agent_id IS NOT NULL
                    AND
                    P.registered_on >= {queryTimeRange.From}
                    AND
                    P.registered_on < {queryTimeRange.To}");

        return query.ToAsyncEnumerable();
    }
}
