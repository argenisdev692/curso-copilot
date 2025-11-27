# Tema 13: Copilot para Código VBA

> **Contexto**: Migración y modernización de código VBA legacy
> **Metodología**: Prompts estructurados con fórmula C.O.R.E.

---

## 📚 Teoría Rápida: VBA en el Contexto Actual

### ¿Por qué Migrar VBA?

| Problema con VBA | Solución Moderna |
|------------------|------------------|
| Sin control de versiones efectivo | Git + CI/CD |
| Difícil de testear | xUnit, pruebas automatizadas |
| Acoplado a Office | APIs independientes, microservicios |
| Sin tipado fuerte | C# con tipos estrictos |
| Mantenimiento costoso | Código modular y documentado |

### Estrategia de Migración

```
┌─────────────────────────────────────────────────────────────┐
│  FLUJO DE MIGRACIÓN VBA → C#                                │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  1. ANÁLISIS     → Entender qué hace el código VBA          │
│  2. DOCUMENTACIÓN → Generar specs del comportamiento        │
│  3. REFACTORIZACIÓN → Limpiar VBA antes de migrar           │
│  4. MIGRACIÓN    → Convertir a C# equivalente               │
│  5. TESTING      → Validar que funciona igual               │
│  6. INTEGRACIÓN  → Conectar con sistemas modernos           │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔍 Redacción y Análisis de Código VBA

### Prompt: Analizar Macro VBA
```
[C] Contexto: Macro VBA en Excel que procesa datos de ventas
[O] Objetivo: Analizar y documentar qué hace el código

Código a analizar:
Sub ProcesarVentas()
    Dim ws As Worksheet
    Dim lastRow As Long
    Set ws = ThisWorkbook.Sheets("Ventas")
    lastRow = ws.Cells(ws.Rows.Count, "A").End(xlUp).Row
    
    For i = 2 To lastRow
        If ws.Cells(i, 3).Value > 1000 Then
            ws.Cells(i, 5).Value = ws.Cells(i, 3).Value * 0.1
        Else
            ws.Cells(i, 5).Value = 0
        End If
    Next i
End Sub

[R] Restricciones:
- Explicación en español
- Identificar inputs, outputs, lógica de negocio
- Detectar posibles bugs o mejoras

[E] Formato:
- Resumen de funcionalidad (1-2 oraciones)
- Tabla: columna → significado
- Lógica de negocio en pseudocódigo
- Posibles problemas detectados
```

### Prompt: Documentar Módulo VBA Completo
```
[C] Contexto: Módulo VBA con múltiples funciones heredadas
[O] Objetivo: Generar documentación técnica completa

Para cada Sub/Function:
- Propósito
- Parámetros (nombre, tipo, descripción)
- Retorno
- Dependencias (otras funciones, objetos)
- Ejemplo de uso

[R] Restricciones:
- NO modificar el código original
- Marcar código sospechoso (posibles bugs)
- Identificar código duplicado

[E] Formato: Markdown con tabla por función
```

### Prompt: Extraer Lógica de Negocio
```
[C] Contexto: VBA con lógica de negocio mezclada con manipulación de Excel
[O] Objetivo: Separar lógica de negocio de lógica de presentación

Identificar:
1. Reglas de negocio (cálculos, validaciones, condiciones)
2. Acceso a datos (lectura/escritura de celdas)
3. Formateo (colores, estilos, fórmulas)

[R] Restricciones:
- Documentar reglas en formato "SI condición ENTONCES acción"
- NO asumir contexto no presente en el código

[E] Formato:
- Lista de reglas de negocio
- Diagrama de flujo (Mermaid)
- Mapeo de celdas utilizadas
```

---

## 🧹 Limpieza y Refactorización de VBA

### Prompt: Detectar Code Smells en VBA
```
[C] Contexto: Código VBA legacy sin mantenimiento
[O] Objetivo: Identificar problemas de calidad

Code smells a buscar:
- Variables sin tipo explícito (Variant implícito)
- GoTo statements
- Funciones muy largas (>50 líneas)
- Código duplicado
- Nombres no descriptivos (a, x, temp)
- Números mágicos sin constantes
- Error handling ausente o incorrecto

[R] Restricciones:
- Priorizar por impacto (High/Medium/Low)
- Sugerir fix para cada problema
- NO cambiar funcionalidad

[E] Formato: Tabla con línea → problema → severidad → solución
```

### Prompt: Refactorizar VBA sin Cambiar Funcionalidad
```
[C] Contexto: Sub VBA con 200 líneas, difícil de mantener
[O] Objetivo: Dividir en funciones pequeñas y claras

Principios a aplicar:
- Single Responsibility: una función = una tarea
- Nombres descriptivos para variables y funciones
- Constantes para números mágicos
- Error handling con On Error GoTo

[R] Restricciones:
- MANTENER comportamiento exacto
- Agregar comentarios explicativos
- Usar Option Explicit
- Tipar todas las variables

[E] Formato:
- Código refactorizado completo
- Lista de funciones extraídas
- Antes/después de una sección ejemplo
```

### Prompt: Agregar Error Handling a VBA
```
[C] Contexto: VBA sin manejo de errores, falla silenciosamente
[O] Objetivo: Agregar error handling robusto

Patrón a aplicar:
Sub MiProcedimiento()
    On Error GoTo ErrorHandler
    ' código principal
    Exit Sub
ErrorHandler:
    MsgBox "Error: " & Err.Description
    ' logging opcional
End Sub

[R] Restricciones:
- NO usar On Error Resume Next (oculta errores)
- Logging a archivo o celda específica
- Limpiar recursos (Close, Set = Nothing)

[E] Formato: Código con error handling agregado
```

---

## 🔄 Migración de VBA a C#

### Prompt: Convertir Sub VBA a Método C#
```
[C] Contexto: Migrar lógica de VBA a biblioteca C# .NET 8
[O] Objetivo: Convertir Sub a método C# equivalente

Código VBA original:
Function CalcularDescuento(monto As Double, tipo As String) As Double
    If tipo = "VIP" Then
        CalcularDescuento = monto * 0.2
    ElseIf tipo = "Regular" Then
        CalcularDescuento = monto * 0.1
    Else
        CalcularDescuento = 0
    End If
End Function

[R] Restricciones:
- Tipado fuerte (no dynamic/object)
- Usar pattern matching si aplica
- Validar parámetros (null checks)
- XML comments para documentación
- Naming conventions de C# (PascalCase)

[E] Formato:
- Método C# completo con documentación
- Enum para tipos si hay valores fijos
- Unit tests básicos
```

### Prompt: Migrar Manipulación de Excel
```
[C] Contexto: VBA que lee/escribe Excel, migrar a C# con EPPlus
[O] Objetivo: Reemplazar objetos VBA por EPPlus

Mapeo de conceptos:
- Workbook → ExcelPackage
- Worksheet → ExcelWorksheet  
- Range/Cells → worksheet.Cells[row, col]
- lastRow → worksheet.Dimension.End.Row

[R] Restricciones:
- Usar EPPlus 7+ (licencia Polyform)
- Dispose de ExcelPackage (using statement)
- NO cargar archivo completo en memoria si es grande
- Async para archivos grandes

[E] Formato:
- Código C# con EPPlus
- Paquete NuGet necesario
- Ejemplo de uso
```

### Prompt: Migrar VBA con Conexión a Base de Datos
```
[C] Contexto: VBA con ADODB para conectar a SQL Server
[O] Objetivo: Migrar a C# con Entity Framework Core o Dapper

Código VBA original:
Dim conn As ADODB.Connection
Set conn = New ADODB.Connection
conn.Open "Provider=SQLOLEDB;Data Source=servidor;..."
Dim rs As ADODB.Recordset
Set rs = conn.Execute("SELECT * FROM Productos")

[R] Restricciones:
- Connection string en appsettings.json
- Using statements para conexiones
- Parámetros SQL (no concatenación)
- Async/await para queries

[E] Formato:
- Repository con Dapper o EF Core
- Modelo de datos (class)
- Configuración de DI
```

### Prompt: Migración Completa de Módulo
```
[C] Contexto: Módulo VBA completo a migrar a proyecto C#
[O] Objetivo: Plan de migración y código equivalente

Fases:
1. Crear proyecto C# (.NET 8 class library)
2. Definir modelos de datos (records/classes)
3. Migrar cada función VBA a método C#
4. Agregar unit tests
5. Crear interfaz de consola/API para testing

[R] Restricciones:
- Mantener 100% funcionalidad
- Documentar diferencias de comportamiento
- NO migrar UI de Excel (solo lógica)

[E] Formato:
- Estructura de proyecto sugerida
- Código migrado por secciones
- Tests para validar equivalencia
```

---

## 🧪 Testing Post-Migración

### Prompt: Generar Tests de Equivalencia
```
[C] Contexto: Función migrada de VBA a C#, validar equivalencia
[O] Objetivo: Tests que comparen output VBA vs C#

Estrategia:
1. Ejecutar función VBA con datos de prueba
2. Ejecutar método C# con mismos datos
3. Comparar resultados

[R] Restricciones:
- Cubrir casos límite (0, null, máximos)
- Documentar diferencias aceptables (redondeo, etc.)
- Automatizar con xUnit + Theory

[E] Formato:
- Tests parametrizados con [Theory]
- Dataset de prueba representativo
- Assertions específicas
```

### Prompt: Casos de Prueba desde VBA
```
[C] Contexto: No hay tests para el VBA original
[O] Objetivo: Derivar casos de prueba del código VBA

Analizar código VBA para identificar:
- Paths de ejecución (if/else branches)
- Valores límite mencionados en condiciones
- Casos de error posibles

[R] Restricciones:
- Al menos un test por branch
- Incluir happy path y error paths
- Nombres descriptivos

[E] Formato:
- Lista de test cases en formato Given/When/Then
- Datos de entrada para cada caso
```

---

## 📊 Ejemplos Prácticos

### Ejemplo 1: Cálculo de Comisiones

**VBA Original:**
```vba
Function CalcularComision(ventas As Double, antiguedad As Integer) As Double
    Dim base As Double
    base = 0.05
    
    If antiguedad > 5 Then
        base = base + 0.02
    End If
    
    If ventas > 100000 Then
        base = base + 0.03
    ElseIf ventas > 50000 Then
        base = base + 0.01
    End If
    
    CalcularComision = ventas * base
End Function
```

### Prompt: Migrar CalcularComision
```
[C] Contexto: Función VBA de cálculo de comisiones, migrar a C#
[O] Objetivo: Método C# con misma lógica pero mejorado

Mejoras a aplicar:
- Extraer tasas a constantes o configuración
- Validar parámetros negativos
- Documentación XML
- Unit tests

[R] Restricciones:
- Resultado numérico idéntico al VBA
- Usar decimal en lugar de double para dinero
- Hacer el cálculo testeable (inyectar configuración)

[E] Formato:
- CommissionCalculator class
- ICommissionConfig interface
- 5+ unit tests
```

### Ejemplo 2: Procesamiento de Archivo

**VBA Original:**
```vba
Sub ImportarDatos()
    Dim ws As Worksheet
    Set ws = ThisWorkbook.Sheets("Datos")
    
    Dim fso As Object
    Set fso = CreateObject("Scripting.FileSystemObject")
    Dim archivo As Object
    Set archivo = fso.OpenTextFile("C:\datos\input.csv", 1)
    
    Dim linea As String
    Dim fila As Integer
    fila = 1
    
    Do Until archivo.AtEndOfStream
        linea = archivo.ReadLine
        Dim campos() As String
        campos = Split(linea, ",")
        ws.Cells(fila, 1).Value = campos(0)
        ws.Cells(fila, 2).Value = campos(1)
        fila = fila + 1
    Loop
    
    archivo.Close
End Sub
```

### Prompt: Migrar ImportarDatos
```
[C] Contexto: VBA que importa CSV a Excel, migrar a C# independiente
[O] Objetivo: Servicio C# para procesar CSV

Cambios arquitectónicos:
- Leer de cualquier stream (no solo archivo)
- Retornar lista de objetos (no escribir a Excel)
- Usar CsvHelper para parsing robusto

[R] Restricciones:
- Manejar encoding UTF-8
- Validar formato de cada campo
- Async para archivos grandes
- Logging de errores por línea

[E] Formato:
- ICsvImportService interface
- CsvImportService implementation
- Modelo de datos para la fila
- Tests con archivos de ejemplo
```

---

## 🎯 Ejercicio Práctico: Migración Completa

```
Secuencia para migrar módulo VBA:

Paso 1: "Analiza este módulo VBA y documenta cada función"
→ Obtener: Documentación completa

Paso 2: "Identifica code smells y refactoriza VBA"
→ Obtener: VBA limpio como base

Paso 3: "Extrae las reglas de negocio en pseudocódigo"
→ Obtener: Especificación para C#

Paso 4: "Migra cada función a C# .NET 8"
→ Obtener: Código C# equivalente

Paso 5: "Genera unit tests para validar equivalencia"
→ Obtener: Suite de tests

Paso 6: "Crea API REST para exponer funcionalidad"
→ Obtener: Servicio moderno
```

---

## 📋 Tabla Resumen: Qué Prompt Usar

| Necesidad | Prompt C.O.R.E. |
|-----------|-----------------|
| Entender VBA | `[C] VBA legacy [O] Documentar funcionalidad [R] No modificar` |
| Limpiar VBA | `[C] VBA con smells [O] Refactorizar [R] Mantener comportamiento` |
| Migrar función | `[C] Function VBA [O] Método C# [R] Tipado, validación` |
| Migrar Excel | `[C] VBA con Excel [O] C# con EPPlus [R] Dispose, async` |
| Migrar DB | `[C] VBA con ADODB [O] C# con EF/Dapper [R] Params, async` |
| Validar | `[C] Migración hecha [O] Tests equivalencia [R] Casos límite` |

---

## 📚 Recursos Adicionales

- [EPPlus Documentation](https://epplussoftware.com/docs)
- [CsvHelper](https://joshclose.github.io/CsvHelper/)
- [Dapper Tutorial](https://dapper-tutorial.net/)
- [VBA to C# Migration Guide - Microsoft](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/)
