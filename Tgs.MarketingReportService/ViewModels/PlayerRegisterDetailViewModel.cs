namespace Tgs.MarketingReportService.ViewModels;

public class PlayerRegisterDetailViewModel
{
    public Guid AgentId { get; init; }

    public DateTime? LastLoginOn { get; init; }

    public int LoginCount { get; init; }

    public string PlayerCode { get; init; } = default!;

    public string PlayerName { get; init; } = default!;

    public DateTime? RegisteredOn { get; init; }
}
