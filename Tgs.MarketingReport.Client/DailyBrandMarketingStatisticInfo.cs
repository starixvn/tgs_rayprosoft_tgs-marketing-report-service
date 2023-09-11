namespace Tgs.MarketingReport.Client;

public record DailyBrandMarketingStatisticInfo
{
    public Guid BrandId { get; init; }

    public int DirectlyRegisterCount { get; init; }
    public int DirectlyGamingCount { get; init; }
    public int TotalGamingCount { get; init; }

    public int TotalRegisterCount { get; init; }

    public DateOnly ReportDate { get; init; }
}
