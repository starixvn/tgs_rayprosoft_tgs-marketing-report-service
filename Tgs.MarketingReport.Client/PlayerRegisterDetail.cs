namespace Tgs.MarketingReport.Client;
public record PlayerRegisterDetail(
    string PlayerCode,
    string PlayerName,
    Guid AgentId,
    DateTime? RegisteredOn,
    DateTime? LastLoginOn,
    int LoginCount);
