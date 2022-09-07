namespace Tgs.MarketingReport;

internal class DefaultSystemClock : ISystemClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
