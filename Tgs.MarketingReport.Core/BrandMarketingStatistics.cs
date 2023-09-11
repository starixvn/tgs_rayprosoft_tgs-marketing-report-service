namespace Tgs.MarketingReport;

public record BrandMarketingStatistics(
    Guid BrandId,
    DateOnly ReportDate,
    int DirectlyGamingCount,
    int DirectlyRegisterCount,
    int TotalRegisterCount,
    int TotalGamingCount);
