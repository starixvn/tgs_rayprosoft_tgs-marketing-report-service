namespace Tgs.MarketingReport.Client;

public record PlayerGamingDetail(
    string PlayerCode,
    string PlayerName,
    Guid AgentId,
    string GpCode,
    string SubGpCode,
    decimal WinLossAmount);
