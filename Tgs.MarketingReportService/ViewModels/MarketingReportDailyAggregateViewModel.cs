namespace Tgs.MarketingReportService.ViewModels;

public class MarketingReportDailyAggregateViewModel
{
    public Guid AgentId { get; init; }

    public int FirstDepositCount { get; init; }

    public int GamingCount { get; init; }

    public int RegisterCount { get; init; }

    public DateOnly ReportDate { get; init; }
}
