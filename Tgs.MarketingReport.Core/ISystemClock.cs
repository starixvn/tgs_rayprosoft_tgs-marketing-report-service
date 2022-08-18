namespace Tgs.MarketingReport;

internal interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
}
