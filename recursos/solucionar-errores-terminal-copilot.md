# 🔧 Solucionar Errores de Terminal con GitHub Copilot

> Guía para resolver errores de compilación y ejecución usando Copilot.

---

## 1. Estrategia General

```
Error en Terminal → Copiar Error → Pegar en Copilot Chat → Obtener Solución
```

---

## 2. Cómo Reportar Errores a Copilot

### ❌ Forma incorrecta
```
"Mi app no funciona"
```

### ✅ Forma correcta
```
Intento iniciar el backend y obtengo este error:

[Pegar error completo de la terminal]

¿Cómo lo soluciono?
```

---

## 3. Plantilla para Reportar Errores

```markdown
**Contexto**: [Qué estaba haciendo]
**Comando ejecutado**: [ej: dotnet run, npm start]
**Error completo**:
```
[Pegar aquí el stack trace completo]
```

**Archivos relevantes**: #file:Program.cs (si aplica)
```

---

## 4. Ejemplos de Errores Comunes

### Error de Dependencia no resuelta (.NET)
```
Unable to resolve service for type 'IService' 
while attempting to activate 'MyController'
```

**Prompt a Copilot:**
```
Este error indica que falta registrar un servicio en DI.
[Pegar error]
Revisa #file:Program.cs y agrega el registro faltante.
```

---

### Error de compilación TypeScript/Angular
```
TS2304: Cannot find name 'Observable'
```

**Prompt:**
```
Error de TypeScript: [error]
¿Qué import falta?
```

---

### Error de conexión a base de datos
```
Connection refused localhost:5432
```

**Prompt:**
```
Error de conexión a PostgreSQL.
¿Está corriendo el servicio? ¿El connection string es correcto?
Revisa #file:appsettings.json
```

---

## 5. Comandos Útiles en Copilot Chat

| Situación | Prompt |
|-----------|--------|
| Error de build | `Explica este error y cómo solucionarlo: [error]` |
| Warning | `¿Cómo elimino este warning? [warning]` |
| Error de runtime | `La app crashea con: [error]. ¿Causa probable?` |
| Múltiples errores | `Tengo estos errores. Prioriza cuál resolver primero` |

---

## 6. Usar Referencias de Archivos

Incluye contexto con `#file:` para que Copilot tenga más información:

```
Error en Program.cs línea 45: [error]
Revisa #file:Program.cs y #file:appsettings.json
```

---

## 7. Tips Pro

### Pedir explicación antes de la solución
```
Explica qué significa este error antes de solucionarlo:
[error]
```

### Solicitar validación
```
Después de aplicar el fix, ¿qué comando debo ejecutar 
para verificar que está resuelto?
```

### Prevenir errores futuros
```
¿Cómo evito este tipo de error en el futuro?
¿Hay alguna configuración o patrón recomendado?
```

---

## 8. Flujo Recomendado

```
1. Copiar error COMPLETO (no resumir)
2. Abrir Copilot Chat (Ctrl+Shift+I)
3. Pegar con contexto mínimo necesario
4. Aplicar solución sugerida
5. Recompilar/ejecutar
6. Si persiste → dar feedback a Copilot con nuevo error
```

---

## 9. Atajos VS Code

| Acción | Atajo |
|--------|-------|
| Abrir Copilot Chat | `Ctrl+Shift+I` |
| Enviar selección a Chat | `Ctrl+Shift+L` |
| Quick fix con Copilot | `Ctrl+.` → Copilot |

---

> 💡 **Recuerda**: Mientras más contexto des (error completo + archivos relevantes), mejor será la solución de Copilot.
