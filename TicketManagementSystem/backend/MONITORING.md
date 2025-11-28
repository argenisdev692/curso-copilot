# Monitorización Completa - Ticket Management System

## Resumen
Este documento describe la implementación completa de monitorización para el backend .NET 8, incluyendo Application Insights, Prometheus, Grafana, alertas y logs centralizados.

## Componentes Implementados

### 1. Application Insights
- **Telemetría**: Requests, dependencies, exceptions, traces
- **Configuración**: En `Program.cs` y `appsettings.json`
- **Pipeline**: Configurado en Azure DevOps para staging y production

### 2. Prometheus Exporters
- **Métricas Custom**:
  - `ticket_management_request_duration_seconds`: Duración de requests
  - `ticket_management_deployments_total`: Número de deployments
  - `ticket_management_tests_total`: Resultados de tests (passed/failed)
  - `ticket_management_incident_resolution_time_seconds`: MTTR
- **Endpoints**:
  - `/metrics`: Métricas estándar de Prometheus
  - `/metrics/custom`: Métricas custom
  - `/metrics/dora`: KPIs DORA

### 3. Grafana Dashboards
Para configurar Grafana:

1. **Agregar Prometheus como Data Source**:
   ```
   URL: http://tu-prometheus-server:9090
   ```

2. **Dashboard DORA Metrics**:
   ```json
   {
     "dashboard": {
       "title": "DORA Metrics - Ticket Management",
       "panels": [
         {
           "title": "Deployment Frequency",
           "type": "stat",
           "targets": [
             {
               "expr": "rate(ticket_management_deployments_total[30d]) * 30",
               "legendFormat": "Deployments per month"
             }
           ]
         },
         {
           "title": "Lead Time for Changes",
           "type": "stat",
           "targets": [
             {
               "expr": "histogram_quantile(0.95, rate(ticket_management_request_duration_seconds_bucket[1h]))",
               "legendFormat": "95th percentile response time"
             }
           ]
         },
         {
           "title": "Change Failure Rate",
           "type": "stat",
           "targets": [
             {
               "expr": "rate(ticket_management_tests_total{result=\"failed\"}[7d]) / rate(ticket_management_tests_total[7d]) * 100",
               "legendFormat": "Failure rate %"
             }
           ]
         },
         {
           "title": "MTTR",
           "type": "stat",
           "targets": [
             {
               "expr": "histogram_quantile(0.50, ticket_management_incident_resolution_time_seconds)",
               "legendFormat": "Median resolution time (hours)"
             }
           ]
         }
       ]
     }
   }
   ```

### 4. Alertas Slack/Teams
- **Configurado en Pipeline**: Envía notificaciones en caso de failure
- **Variables requeridas**:
  - `slackWebhookUrl`: URL del webhook de Slack
  - `teamsWebhookUrl`: URL del webhook de Teams (opcional)

### 5. Logs Centralizados ELK
- **Serilog Sink**: Configurado para Elasticsearch
- **Configuración**: En `appsettings.json`
- **Stack ELK**:
  - Elasticsearch: Almacenamiento de logs
  - Logstash: Procesamiento (opcional)
  - Kibana: Visualización de logs

## Configuración Requerida

### Variables de Azure DevOps
```
ApplicationInsightsInstrumentationKey: tu-key-aqui
ApplicationInsightsConnectionString: InstrumentationKey=tu-key;...
slackWebhookUrl: https://hooks.slack.com/services/...
```

### Variables de Entorno
```bash
# Application Insights
APPINSIGHTS_INSTRUMENTATIONKEY=tu-key
ApplicationInsights__ConnectionString=tu-connection-string

# Elasticsearch
ELASTICSEARCH_NODE_URIS=http://localhost:9200

# Prometheus
MONITORING__ENABLEPROMETHEUS=true
```

## Endpoints de Monitorización

- `/health`: Health checks
- `/metrics`: Métricas Prometheus
- `/metrics/custom`: Métricas custom
- `/metrics/dora`: KPIs DORA

## Próximos Pasos

1. Configurar Grafana dashboard con las métricas
2. Configurar alertas en Grafana para thresholds
3. Implementar tracking de release velocity en el pipeline
4. Agregar métricas de business (tickets creados, resueltos, etc.)

## Comandos para Ejecutar

```bash
# Restaurar dependencias
dotnet restore

# Ejecutar aplicación
dotnet run

# Ver métricas en navegador
# http://localhost:5000/metrics
# http://localhost:5000/health
```