#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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
RUN dotnet build "./Tgs.MarketingReportService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.MarketingReportService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.MarketingReportService.dll"]