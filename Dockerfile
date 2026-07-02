# ── Stage 1: build ──────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar solution y proyectos para restaurar dependencias
COPY Zounduct.sln .
COPY src/Zounduct.Api/Zounduct.Api.csproj             src/Zounduct.Api/
COPY src/Zounduct.Application/Zounduct.Application.csproj   src/Zounduct.Application/
COPY src/Zounduct.Domain/Zounduct.Domain.csproj           src/Zounduct.Domain/
COPY src/Zounduct.Infrastructure/Zounduct.Infrastructure.csproj src/Zounduct.Infrastructure/

RUN dotnet restore

# Copiar el resto del código y publicar
COPY src/ src/
RUN dotnet publish src/Zounduct.Api/Zounduct.Api.csproj -c Release -o /app/publish --no-restore

# ── Stage 2: runtime ─────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Zounduct.Api.dll"]
