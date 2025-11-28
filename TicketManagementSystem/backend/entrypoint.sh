#!/bin/bash
# Script de entrada para ejecutar migraciones EF Core y luego iniciar la aplicación

set -e

echo "Ejecutando migraciones de base de datos..."
dotnet ef database update --no-build

echo "Iniciando aplicación..."
exec dotnet TicketManagementSystem.API.dll