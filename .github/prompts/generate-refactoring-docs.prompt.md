---
description: 'Genera documentación técnica automática: README, diagramas, JSDoc/XML comments, migration guides'
---

# Generador de Documentación Técnica

## 🎯 Propósito
Generar documentación técnica completa y profesional para código refactorizado, nuevas features o proyectos completos.

## 📋 Tipos de Documentación a Generar

### 1. README.md de Proyecto
Debe incluir:
- Descripción general del propósito del proyecto
- Arquitectura del sistema (diagrama Mermaid)
- Lista de tecnologías utilizadas (Backend y Frontend)
- Instrucciones detalladas de instalación
- Configuración requerida (variables de entorno, appsettings, environments)
- Ejemplos de uso básico
- Guía de troubleshooting para errores comunes
- Enlaces a documentación adicional

### 2. README.md de Feature/Módulo
Para cada módulo funcional, incluir:
- Propósito específico del módulo
- Componentes incluidos (Controllers, Services, Components)
- Dependencias internas y externas
- Flujos principales de ejecución
- API endpoints expuestos (si aplica)
- Diagramas de secuencia para flujos complejos

### 3. Diagramas Técnicos (Mermaid)
Generar diagramas para:
- **Arquitectura de sistema**: Mostrar capas (Frontend, API, Business Logic, Data Layer, External Services)
- **Diagramas de flujo**: Para procesos de negocio complejos
- **Diagramas de secuencia**: Para flujos de autenticación, transacciones, etc.
- **Entity Relationship Diagrams**: Para estructura de base de datos

**Formato**: Usar sintaxis Mermaid embebida en markdown

### 4. XML Comments (C#)
Para todo código C# público, documentar con:
- `<summary>`: Descripción clara de la clase/método
- `<param name="">`: Explicación de cada parámetro
- `<returns>`: Qué retorna el método y en qué casos
- `<exception cref="">`: Qué excepciones puede lanzar
- `<remarks>`: Notas adicionales sobre comportamiento especial
- `<example>`: Ejemplos de uso cuando el método sea complejo
- `<seealso cref="">`: Referencias a métodos relacionados

### 5. JSDoc (TypeScript/JavaScript)
Para código TypeScript/JavaScript exportado:
- `@class`, `@interface`: Documentar clases e interfaces
- `@param {type}`: Tipo y descripción de parámetros
- `@returns {type}`: Tipo y descripción del retorno
- `@throws {Error}`: Errores que puede lanzar
- `@example`: Ejemplo de uso con syntax highlighting
- `@deprecated`: Si la función está deprecada, indicar alternativa
- `@see`: Referencias a documentación relacionada

### 6. Migration Guides
Cuando hay cambios breaking, documentar:
- **Resumen de cambios**: Lista de qué cambió entre versiones
- **Cambios Breaking**: Detallar cada cambio con ejemplos antes/después (solo estructura)
- **Estructura de Base de Datos**: Nuevas tablas, columnas modificadas, describir migraciones
- **Cambios en API**: Endpoints modificados, nuevos campos, campos eliminados
- **Pasos de migración**: Instrucciones paso a paso para Backend y Frontend
- **Rollback Plan**: Cómo revertir si algo falla
- **Checklist**: Lista verificable de tareas de migración

### 7. API Documentation
Para endpoints REST, documentar:
- Lista completa de endpoints disponibles
- Método HTTP (GET, POST, PUT, DELETE, PATCH)
- Request schemas con tipos de datos
- Response schemas con códigos de estado (200, 201, 400, 404, 500)
- Estructura de request/response (sin código de implementación)
- Autenticación requerida (Bearer token, API key)
- Rate limiting y throttling

## 🔧 Estructura de README Principal

Un README.md de proyecto debe contener estas secciones en orden:

1. **Título y Badges** (opcional: build status, coverage, version)
2. **Descripción**: 2-3 párrafos explicando qué hace el proyecto
3. **Arquitectura**: Diagrama Mermaid mostrando componentes principales
4. **Tecnologías**: Lista de frameworks, librerías y versiones
5. **Requisitos Previos**: Software necesario antes de instalar
6. **Instalación**: Pasos para configurar Backend y Frontend
7. **Configuración**: Archivos de config, variables de entorno con tabla descriptiva
8. **Ejecución**: Cómo correr el proyecto localmente
9. **Testing**: Comandos para ejecutar tests
10. **Documentación Adicional**: Enlaces a docs específicas
11. **Troubleshooting**: Problemas comunes y soluciones
12. **Licencia**: Tipo de licencia del proyecto
13. **Contribuidores**: Lista de desarrolladores principales

## 📐 Estructura de Diagramas Mermaid

### Diagrama de Arquitectura
Debe mostrar claramente:
- Capa de presentación (Frontend)
- Capa de API (Controllers, Middleware)
- Capa de lógica de negocio (Services, Validators)
- Capa de datos (Repositories, DbContext)
- Servicios externos (SMTP, Storage, Cache)
- Flujo de datos entre capas con flechas direccionales

### Diagrama de Secuencia
Para flujos como autenticación, debe incluir:
- Participantes (Usuario, Frontend, API, Database)
- Secuencia de mensajes entre participantes
- Respuestas y flujos alternativos
- Manejo de errores si es relevante

### Entity Relationship Diagram
Debe mostrar:
- Todas las entidades principales del dominio
- Relaciones entre entidades (1:1, 1:N, N:M)
- Cardinalidad explícita
- Primary Keys (PK) y Foreign Keys (FK)
- Campos principales de cada entidad

## ✅ Checklist de Documentación Completa

Al generar documentación, verificar que incluya:

- [ ] README.md con descripción clara del proyecto
- [ ] Diagrama de arquitectura general (Mermaid)
- [ ] Diagramas de secuencia para flujos críticos
- [ ] Entity Relationship Diagram de la base de datos
- [ ] XML comments en todos los métodos públicos de C#
- [ ] JSDoc en todas las funciones/clases exportadas de TypeScript
- [ ] Descripción de uso para funcionalidades complejas
- [ ] Tabla de variables de entorno con descripciones
- [ ] Instrucciones de instalación paso a paso
- [ ] Guía de troubleshooting con errores comunes
- [ ] Migration guide si hay cambios breaking
- [ ] API documentation con estructuras de request/response
- [ ] Enlaces a documentación adicional

## 🎯 Formato de Prompt para Copilot

```
Genera documentación técnica completa para [nombre del proyecto/módulo]:

**Contexto:**
- Tecnología: [.NET 8, Angular 17, etc.]
- Propósito: [descripción breve]
- Alcance: [Project completo / Feature específica / Módulo]

**Documentación requerida:**
- README.md principal con arquitectura Mermaid
- Diagramas de secuencia para [flujos específicos]
- Entity Relationship Diagram
- XML Comments en [archivos específicos]
- JSDoc en [archivos específicos]
- Migration Guide de [versión] a [versión]
- API Documentation para endpoints de [módulo]

**Formato:**
- Markdown estructurado
- Diagramas Mermaid renderizables
- Descripciones claras sin código de implementación
- Tablas para variables de entorno y configuración

Archivos a documentar: [#file, #selection]
```
