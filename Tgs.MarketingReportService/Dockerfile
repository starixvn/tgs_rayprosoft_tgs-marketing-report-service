FROM hub.lumigame.com/base/dotnet/core/sdk:8.0 AS builder

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.MarketingReportService/Tgs.MarketingReportService.csproj", "Tgs.MarketingReportService/"]
COPY ["Tgs.MarketingReport.Core.Cache/Tgs.MarketingReport.Core.Cache.csproj", "Tgs.MarketingReport.Core.Cache/"]
COPY ["Tgs.MarketingReport.Core/Tgs.MarketingReport.Core.csproj", "Tgs.MarketingReport.Core/"]
COPY ["Tgs.MarketingReport.Repositories/Tgs.MarketingReport.Repositories.csproj", "Tgs.MarketingReport.Repositories/"]
RUN dotnet restore "./Tgs.MarketingReportService/Tgs.MarketingReportService.csproj"
COPY . .
WORKDIR "/src/Tgs.MarketingReportService"
RUN dotnet publish "./Tgs.MarketingReportService.csproj" -c $BUILD_CONFIGURATION -o /app /p:UseAppHost=false

FROM hub.lumigame.com/base/dotnet/core/aspnet:8.0-alpine-24h1.1 AS base
WORKDIR /app
COPY --from=builder /app .

ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "Tgs.MarketingReportService.dll"]