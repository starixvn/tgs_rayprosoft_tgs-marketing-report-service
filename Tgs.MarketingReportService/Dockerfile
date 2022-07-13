#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BuildConfiguration=Release
RUN --mount=type=secret,id=nuget,dst=/pat cp /pat ~/.nuget/NuGet/NuGet.Config
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.MarketingReportService/Tgs.MarketingReportService.csproj", "Tgs.MarketingReportService/"]
COPY ["Tgs.MarketingReport.Core/Tgs.MarketingReport.Core.csproj", "Tgs.MarketingReport.Core/"]
COPY ["Tgs.MarketingReport.Repositories/Tgs.MarketingReport.Repositories.csproj", "Tgs.MarketingReport.Repositories/"]
COPY ["Tgs.MarketingReport.Core.Cache/Tgs.MarketingReport.Core.Cache.csproj", "Tgs.MarketingReport.Core.Cache/"]
RUN dotnet restore "Tgs.MarketingReportService/Tgs.MarketingReportService.csproj"
COPY . .
WORKDIR "/src/Tgs.MarketingReportService"
RUN dotnet build "Tgs.MarketingReportService.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.MarketingReportService.csproj" -c $BuildConfiguration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.MarketingReportService.dll"]