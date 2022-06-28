namespace Tgs.MarketingReport.Core;

public interface ITgsRepository
{
    IAsyncEnumerable<DailyAggregateInfo> GetDailyAggregateInfosAsync(
        Guid brandId,
        DateOnly startDate,
        DateOnly endDate);
}