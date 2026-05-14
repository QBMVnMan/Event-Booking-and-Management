FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
# Add environment variables to resolve mmap issues with .NET 10
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
ENV DOTNET_NOLOGO=1
# Critical mmap fix for Docker Desktop WSL2
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENV DOTNET_EnableDiagnostics=0
ENV DOTNET_UsePollingFileWatcher=1
# Aggressive memory management for .NET 10
ENV DOTNET_GCHeapHardLimit=0x80000000
ENV DOTNET_GCConserveMemory=1
ENV DOTNET_GCRetainVM=0

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
# Add environment variables for build stage
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
ENV DOTNET_NOLOGO=1
# Disable NuGet cache to avoid mmap issues
ENV NUGET_XMLDOC_MODE=skip
# Critical mmap fix for Docker Desktop WSL2
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENV DOTNET_EnableDiagnostics=0
ENV DOTNET_UsePollingFileWatcher=1
ENV DOTNET_DbgMiniDumpType=4
# Aggressive memory management for .NET 10
ENV DOTNET_GCHeapHardLimit=0x80000000
ENV DOTNET_GCConserveMemory=1
ENV DOTNET_GCRetainVM=0

# ── 1. Copy ALL .csproj files that are referenced (including shared) ──
COPY ["api-gateway/src/ApiGateway/ApiGateway.csproj", "api-gateway/src/ApiGateway/"]

# ── 2. Restore everything with optimized flags ──
RUN dotnet restore "api-gateway/src/ApiGateway/ApiGateway.csproj" --no-cache

# ── 3. Copy the rest of the source code ──
COPY . .

# ── 4. Publish only the service project ──
WORKDIR /src/api-gateway/src/ApiGateway
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]