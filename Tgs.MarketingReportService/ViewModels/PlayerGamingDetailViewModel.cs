namespace Tgs.MarketingReportService.ViewModels;

public class PlayerGamingDetailViewModel
{
    public Guid AgentId { get; init; }

    public string GpCode { get; init; } = default!;

    public string PlayerCode { get; init; } = default!;

    public string PlayerName { get; init; } = default!;

    public string SubGpCode { get; init; } = default!;

    public decimal WinLossAmount { get; init; }
}
