namespace Tgs.MarketingReport.Core.Cache;

public interface ISystemClock
{
    public DateTimeOffset UtcNow { get; }
}
