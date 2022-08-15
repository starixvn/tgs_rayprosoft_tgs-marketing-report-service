using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using DateOnlyTimeOnly.AspNet.Converters;

namespace Tgs.MarketingReport.Client;

internal class MarketingReportClient : IMarketingReportClient
{
    private static readonly JsonSerializerOptions _SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters = {
            new JsonStringEnumConverter(),
            new DateOnlyJsonConverter()
        }
    };

    private readonly HttpClient _httpClient;

    public MarketingReportClient(
        HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async IAsyncEnumerable<DailyAggregateInfo> GetDailyAggregateInfosAsync(
        Guid brandId,
        DateOnly startDate,
        DateOnly endDate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"/marketingreport/{brandId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        var contentJson = await response.Content
            .ReadAsStringAsync(cancellationToken)
            .ConfigureAwait(false);

        var aggregateInfos = JsonSerializer.Deserialize<DailyAggregateInfo[]>(contentJson, _SerializerOptions);

        foreach (var info in aggregateInfos!)
            yield return info;
    }

    public async IAsyncEnumerable<DailyAggregateInfo> GetDailyAggregateInfosAsync(
        Guid brandId,
        Guid agentId,
        DateOnly startDate,
        DateOnly endDate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"/marketingreport/{brandId}/{agentId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        var contentJson = await response.Content
            .ReadAsStringAsync(cancellationToken)
            .ConfigureAwait(false);

        var aggregateInfos = JsonSerializer.Deserialize<DailyAggregateInfo[]>(contentJson, _SerializerOptions);

        foreach (var info in aggregateInfos!)
            yield return info;
    }

    public async IAsyncEnumerable<DailyAggregateInfo> QueryMarketingStatisticsAsync(
        Guid brandId,
        DateOnly queryDate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"/report/marketing/{brandId}/{queryDate:yyyy-MM-dd}", cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        var contentJson = await response.Content
            .ReadAsStringAsync(cancellationToken)
            .ConfigureAwait(false);

        var aggregateInfos = JsonSerializer.Deserialize<DailyAggregateInfo[]>(contentJson, _SerializerOptions);

        foreach (var info in aggregateInfos!)
            yield return info;
    }
}
