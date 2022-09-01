namespace Tgs.MarketingReport;
public record PlayerRegisterItem(
    string PlayerCode,
    string PlayerName,
    Guid AgentId,
    DateTime? RegisteredOn,
    DateTime? LastLoginOn,
    int LoginCount);
