# =========================
# Base Runtime Image
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 8080

# =========================
# Build Image
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["OnlineStore.csproj", "./"]
RUN dotnet restore "OnlineStore.csproj"

COPY . .

RUN dotnet publish "OnlineStore.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# Final Image
# =========================
FROM base AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OnlineStore.dll"]