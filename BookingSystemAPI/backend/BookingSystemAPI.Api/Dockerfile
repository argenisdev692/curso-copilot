# =============================================================================
# Dockerfile Multi-Stage para BookingSystemAPI
# .NET 8 Web API - Producción Ready
# =============================================================================

# -----------------------------------------------------------------------------
# Stage 1: Build - SDK completo para compilar la aplicación
# -----------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Argumentos de compilación
ARG CONFIGURATION=Release

WORKDIR /src

# Copiar archivos de proyecto primero para aprovechar el cache de Docker
COPY ["BookingSystemAPI.Api/BookingSystemAPI.Api.csproj", "BookingSystemAPI.Api/"]

# Copiar el código fuente completo
COPY . .

# Cambiar al directorio del proyecto
WORKDIR "/src/BookingSystemAPI.Api"

# Restaurar dependencias como capa separada (mejor cache)
RUN dotnet restore "BookingSystemAPI.Api.csproj"

# Compilar la aplicación
RUN dotnet build "BookingSystemAPI.Api.csproj" \
    --configuration $CONFIGURATION

# -----------------------------------------------------------------------------
# Stage 2: Publish - Generar artefactos optimizados
# -----------------------------------------------------------------------------
FROM build AS publish

ARG CONFIGURATION=Release

# Publicar la aplicación optimizada para producción
RUN dotnet publish "BookingSystemAPI.Api.csproj" \
    --configuration $CONFIGURATION \
    --output /app/publish

# -----------------------------------------------------------------------------
# Stage 3: Runtime - Imagen final mínima
# -----------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Etiquetas de metadata
LABEL maintainer="BookingSystem Team"
LABEL description="BookingSystemAPI - Sistema de Reservas"
LABEL version="1.0.0"

# Variables de entorno para .NET
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_EnableDiagnostics=0 \
    # Deshabilitar telemetría
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    # Configuración de cultura invariante (mejor rendimiento)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    # Configuración de GC para contenedores
    DOTNET_gcServer=1 \
    DOTNET_GCHeapHardLimit=0x10000000

# Instalar dependencias necesarias para health checks y diagnósticos
RUN apt-get update && apt-get install -y --no-install-recommends \
    curl \
    && rm -rf /var/lib/apt/lists/* \
    && apt-get clean

# Crear usuario no-root para seguridad
RUN groupadd --gid 1000 appgroup \
    && useradd --uid 1000 --gid appgroup --shell /bin/false --create-home appuser

# Establecer directorio de trabajo
WORKDIR /app

# Copiar artefactos publicados desde la etapa de publish
COPY --from=publish --chown=appuser:appgroup /app/publish .

# Cambiar a usuario no-root
USER appuser

# Exponer el puerto de la aplicación
EXPOSE 8080

# Health check para monitoreo de contenedores
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl --fail http://localhost:8080/health || exit 1

# Punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "BookingSystemAPI.Api.dll"]
