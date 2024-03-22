using FluentdForward.OpenTelemetry.MessagePack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .AddOpenTelemetry(options =>
    {
        options.IncludeFormattedMessage = true;
        options.ParseStateValues = true;
        _ = options.AddFluentdForwardExporter(fluentOptions =>
        {
            var nodeIp = builder.Configuration.GetValue<string>("NODE_IP");
            fluentOptions.Host = nodeIp!;
            fluentOptions.Tag = builder.Configuration.GetValue<string>("OTEL_SERVICE_NAME")!;
            fluentOptions.UseMessagePack(LogRecordFormatterResolver.GetResolverInstanceWithExtendInfo(
                "MachineInfo",
                () => new
                {
                    Environment.MachineName
                })
);
        });
    });

builder.Services
    .AddMarketingReportCore((options, sp) =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        config.GetSection("Schedule").Bind(options);
    })
    .AddMarketingReportRepository((sp, dbBuilder) =>
    {
        var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("Tgs");
        _ = dbBuilder.UseNpgsql(connectionString);
    })
    .AddMemoryCache()
    .AddMarketingReportCache();

builder.Services
    .AddRouting(options => options.LowercaseUrls = true)
    .AddControllers();

var excludeExtensions = new[] { "html", "ico", "png", "jpg" };
builder.Services
    .AddSingleton<IConfigureOptions<OtlpExporterOptions>>(
        sp => new ConfigureNamedOptions<OtlpExporterOptions>(
            Options.DefaultName,
            exporterOptions =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var nodeIp = config.GetValue<string>("NODE_IP");

                if (!string.IsNullOrWhiteSpace(nodeIp))
                    exporterOptions.Endpoint = new Uri($"http://{nodeIp}:4317");

                config.GetSection("Opentelemetry:Tracing").Bind(exporterOptions);
            }))
    .AddOpenTelemetry()
    .WithTracing(traceBuilder => traceBuilder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddEnvironmentVariableDetector())
        .AddSource("Tgs.*")
        .AddSource("TGS.*")
        .AddSource("Valhalla.*")
        .AddAspNetCoreInstrumentation(options => options.Filter =
            httpContext => !httpContext.Request.Path.HasValue
                || !httpContext.Request.Path.StartsWithSegments("/swagger")
                && !httpContext.Request.Path.StartsWithSegments("/healthz")
                && !httpContext.Request.Path.StartsWithSegments("/profiler/")
                && !excludeExtensions.Any(
                    ext => httpContext.Request.Path.Value.EndsWith($".{ext}")))
        .AddHttpClientInstrumentation()
        .AddProfileViewExporter()
        .AddOtlpExporter())
    .WithMetrics(meterBuiler => meterBuiler
        .SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddEnvironmentVariableDetector())
        .AddRuntimeInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter())
    .Services
    .AddProfileViewer();

builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tgs.MarketingReportService", Version = "v1" });
    });

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapControllers();
app.MapProfileViewer();
app.MapSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/healthz");

app.Run();
