FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# ── 1. Copy ALL .csproj files that are referenced (including shared) ──
COPY ["shared/Contracts/Contracts.csproj", "shared/Contracts/"]
COPY ["services/EventService/src/EventService.Api/EventService.Api.csproj", "services/EventService/src/EventService.Api/"]


# ── 2. Restore everything ──
RUN dotnet restore "shared/Contracts/Contracts.csproj"
RUN dotnet restore "services/EventService/src/EventService.Api/EventService.Api.csproj"

# ── 3. Copy the rest of the source code ──
COPY . .

# ── 4. Publish only the service project ──
WORKDIR /src/services/EventService/src/EventService.Api
RUN dotnet publish . -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EventService.Api.dll"]