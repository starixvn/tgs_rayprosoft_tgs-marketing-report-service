namespace Tgs.MarketingReport;
public record PlayerGamingItem(
    string PlayerCode,
    string PlayerName,
    Guid AgentId,
    string GpCode,
    string SubGpCode,
    decimal WinLossAmount);
