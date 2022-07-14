namespace Tgs.MarketingReport.Core.Cache;

internal class DefaultSystemClock : ISystemClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
