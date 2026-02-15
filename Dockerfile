FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY MonitoringService/MonitoringService.Api.csproj MonitoringService/
COPY MonitoringService.Core/MonitoringService.Core.csproj MonitoringService.Core/
COPY MonitoringService.Infrastructure/MonitoringService.Infrastructure.csproj MonitoringService.Infrastructure/

RUN dotnet restore MonitoringService/MonitoringService.Api.csproj

COPY . .
RUN dotnet publish MonitoringService/MonitoringService.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "MonitoringService.Api.dll"]