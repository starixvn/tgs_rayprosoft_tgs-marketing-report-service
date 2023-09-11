namespace Tgs.MarketingReportService.ViewModels;

public class BrandMarketingStatisticViewModel
{
    public Guid BrandId { get; init; }

    public int DirectlyGamingCount { get; init; }

    public int DirectlyRegisterCount { get; init; }

    public DateOnly ReportDate { get; init; }

    public int TotalGamingCount { get; init; }

    public int TotalRegisterCount { get; init; }
}
