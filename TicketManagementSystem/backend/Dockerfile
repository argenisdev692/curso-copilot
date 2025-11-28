# Dockerfile multi-stage para API .NET 8 con EF Core
# Etapa 1: Build - Usamos SDK para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["TicketManagementSystem.API/TicketManagementSystem.API.csproj", "TicketManagementSystem.API/"]
RUN dotnet restore "TicketManagementSystem.API/TicketManagementSystem.API.csproj"

# Copiar el resto del código fuente
COPY . .

# Compilar la aplicación en modo Release
WORKDIR "/src/TicketManagementSystem.API"
RUN dotnet build "TicketManagementSystem.API.csproj" -c Release -o /app/build

# Publicar la aplicación
RUN dotnet publish "TicketManagementSystem.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime - Imagen ligera para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Instalar curl para health checks
USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER app

# Copiar la aplicación publicada
COPY --from=build /app/publish .

# Instalar dotnet-ef tool para migraciones automáticas
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

# Copiar script de entrada
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

# Exponer puertos HTTP y HTTPS
EXPOSE 80
EXPOSE 443

# Health check: Verifica que la API responda en /health
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost/health || exit 1

# Usuario no root por seguridad
USER app

# Punto de entrada: Ejecutar script que hace migraciones y luego la app
ENTRYPOINT ["./entrypoint.sh"]