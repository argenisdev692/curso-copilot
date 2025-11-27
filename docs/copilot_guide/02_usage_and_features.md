# 💡 Guía de Uso y Funcionalidades - GitHub Copilot (Noviembre 2025)

## 🎯 Consejos Prácticos para Usar GitHub Copilot Efectivamente

Esta guía te ayudará a aprovechar al máximo GitHub Copilot en tus proyectos de desarrollo, incluyendo las nuevas funcionalidades de noviembre 2025 como Plan Mode, subagentes y Agent Sessions.

> ⚠️ **Actualización Modelo - Noviembre 2025:**
> - **Modelo Predeterminado:** GPT-4.1 (mejor rendimiento que GPT-4o)
> - **Nuevo Modelo:** GPT-5-Codex disponible en planes Pro+
> - **Mejoras:** Mejor integración con herramientas, contextos más precisos

## 🚀 Funcionalidades Principales (Noviembre 2025)

### Autocompletado Inteligente

GitHub Copilot ofrece autocompletado avanzado basado en:

- **Contexto del código:** Entiende el lenguaje, librerías y patrones
- **Proyecto completo:** Aprende de tu base de código
- **Tendencias globales:** Datos de miles de millones de líneas de código público
- **Modelos GPT-4.1/GPT-5-Codex:** Modelos optimizados para desarrollo con mejor precisión
- **MCP Integrado:** Contexto expandido mediante servidores MCP

### Sugerencias Multi-línea y Agent Mode

GitHub Copilot puede sugerir bloques completos de código y ejecutar tareas complejas de forma autónoma:

```javascript
// Ejemplo: GitHub Copilot sugiere funciones completas
function fetchUserData(userId) {
  return fetch(`/api/users/${userId}`)
    .then((response) => response.json())
    .then((data) => {
      if (data.error) {
        throw new Error(data.error.message);
      }
      return data;
    })
    .catch((error) => {
      console.error("Error fetching user data:", error);
      throw error;
    });
}
```

### Plan Mode (2025)
- **Activación**: Selecciona "Plan" en el dropdown de agentes del chat
- **Capacidades**: Investigación y planificación estructurada de tareas complejas
- **Uso**: "Planifica el desarrollo de una aplicación fullstack con .NET y Angular"

### Subagentes (2025)
- **Activación**: Usa `#runSubagent` en prompts para delegar tareas
- **Capacidades**: Procesamiento en segundo plano, análisis especializado
- **Ventajas**: Mejor gestión de contexto, tareas paralelas

### Agent Sessions (2025)
- **Vista Unificada**: Gestiona sesiones locales y en la nube
- **Integraciones**: Soporte para OpenAI Codex y GitHub Copilot CLI
- **Seguimiento**: Monitoreo de progreso en tiempo real

## 💡 Mejores Prácticas de Uso (2025)

### 1. Escribe con Intención

```csharp
// ❌ Mal: Escribe código vago
var x = GetSometh

// ✅ Bien: Sé específico en tu intención
var userRepository = _userRepository.GetUserById(userId);
```

### 2. Aprovecha los Contextos y MCP

GitHub Copilot funciona mejor cuando:

- **Nombres descriptivos:** `calculateTotalPrice()` vs `calc()`
- **Comentarios claros:** Explica qué hace la función
- **Estructuras consistentes:** Sigue patrones de tu proyecto
- **MCP integrado:** Conecta con tus APIs y servicios personalizados vía marketplace
- **Subagentes:** Delega tareas especializadas para mejor contexto

### 3. Aceptación Inteligente y Nuevos Modos

**Métodos de aceptación:**

- `Tab` - Acepta sugerencia completa
- `Ctrl + →` - Acepta palabra por palabra
- `Ctrl + Shift + →` - Rechaza sugerencia
- `Ctrl + Enter` - Abre chat para conversaciones avanzadas
- **Plan Mode:** Investigación y planificación estructurada
- **Subagentes:** Delegación de tareas especializadas

## 🎨 Funcionalidades Avanzadas (2025)

### Auto-Imports Inteligentes

GitHub Copilot sugiere automáticamente los imports necesarios:

```typescript
// Escribes:
const users = await api.getUsers();

// GitHub Copilot sugiere automáticamente:
import { api } from "../services/api";
```

### Completado por Archivos y MCP

GitHub Copilot aprende de archivos relacionados y se integra con MCP:

```
📁 proyecto/
├── models/
│   └── User.cs
├── services/
│   └── UserService.cs  ← GitHub Copilot conoce User.cs
├── apis/
│   └── external-api.ts  ← MCP puede conectar aquí
└── controllers/
    └── UserController.cs
```

### Copilot Edits Mejorado (2025)
- **Refactorización multi-archivo**: Cambia código a través de múltiples archivos
- **Commit messages IA**: Genera mensajes de commit inteligentes
- **Code review avanzado**: Integración con CodeQL/ESLint, handoff directo a coding agent
- **Agent Sessions**: Seguimiento de cambios en sesiones complejas

### Sugerencias Contextuales

Adapta sugerencias según el contexto:

- **Tests:** Sugiere assertions y mocks
- **APIs:** Endpoints, validaciones, respuestas
- **Frontend:** Componentes, estados, eventos
- **Backend:** Queries, models, business logic

## ⚡ Optimización del Rendimiento

### Configuraciones Recomendadas (Noviembre 2025)

```json
{
  "github.copilot.chat.customOAIModels": [],
  "chat.agent.thinkingStyle": "expanded",
  "chat.mcp.autostart": true,
  "chat.tools.terminal.autoReplyToPrompts": true
}
```

### Para Proyectos Grandes

```json
{
  "github.copilot.enable": {
    "*": true,
    "cpp": false
  },
  "chat.useNestedAgentsMdFiles": true,
  "chat.mcp.gallery.enabled": true
}
```

## 🔧 Casos de Uso por Lenguaje

### JavaScript/TypeScript + React

**Funcionalidades destacadas:**

- Auto-imports de componentes
- Sugerencias de hooks
- Completado de JSX
- Props y state management

```jsx
// GitHub Copilot sugiere:
function UserProfile({ user, onUpdate }) {
  const [isEditing, setIsEditing] = useState(false);

  return (
    <div className="user-profile">
      {isEditing ? (
        <UserEditForm
          user={user}
          onSave={(updatedUser) => {
            onUpdate(updatedUser);
            setIsEditing(false);
          }}
          onCancel={() => setIsEditing(false)}
        />
      ) : (
        <UserDisplay user={user} onEdit={() => setIsEditing(true)} />
      )}
    </div>
  );
}
```

### C# + .NET

**Funcionalidades destacadas:**

- LINQ queries avanzadas
- Entity Framework Core
- Dependency injection
- Async/await patterns
- Minimal APIs (.NET 8+)
- Source generators

```csharp
// GitHub Copilot sugiere con .NET 8:
public async Task<IActionResult> GetUser(int id)
{
    var user = await _context.Users
        .Include(u => u.Profile)
        .Include(u => u.Roles)
        .FirstOrDefaultAsync(u => u.Id == id);

    if (user == null)
    {
        return NotFound();
    }

    return Ok(_mapper.Map<UserDto>(user));
}
```

### Angular + TypeScript (2025)

**Funcionalidades destacadas:**

- Componentes standalone
- Signals y control flow
- Angular Material
- RxJS operators
- Testing con Jest
- SSR/SSG con Angular Universal

```typescript
// GitHub Copilot sugiere con Angular 18+:
@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, MatTableModule],
  template: `
    @if (users$ | async; as users) {
      <table mat-table [dataSource]="users">
        <!-- Table content -->
      </table>
    } @else {
      <p>Loading users...</p>
    }
  `
})
export class UserListComponent {
  users$ = this.userService.getUsers();

  constructor(private userService: UserService) {}
}
```

### Python

**Funcionalidades destacadas:**

- Type hints
- Django/Flask patterns
- Data science libraries
- Async functions

```python
# GitHub Copilot sugiere:
from typing import List, Optional
from sqlalchemy.orm import Session
from fastapi import Depends, HTTPException

def get_user_by_email(db: Session, email: str) -> Optional[User]:
    """Get user by email address."""
    return db.query(User).filter(User.email == email).first()

@app.get("/users/{user_id}", response_model=UserResponse)
def read_user(user_id: int, db: Session = Depends(get_db)):
    db_user = get_user_by_id(db, user_id)
    if db_user is None:
        raise HTTPException(status_code=404, detail="User not found")
    return db_user
```

## 🚨 Situaciones Problemáticas

### Cuando GitHub Copilot no Sugiere

**Posibles causas:**

1. **Contexto insuficiente:** Escribe más código antes
2. **Nombres genéricos:** Usa nombres más específicos
3. **Archivos ignorados:** Revisa configuración de exclusiones
4. **Conexión:** Verifica conexión a internet
5. **Límite alcanzado:** Revisa uso mensual (2,000 completions gratis)

### Sugerencias Incorrectas

**Cómo mejorar:**

1. **Entrenamiento:** GitHub Copilot aprende de tus aceptaciones/rechazos
2. **Feedback:** Usa `Ctrl + Shift + →` para rechazar
3. **Context:** Proporciona más contexto en el código
4. **Plan Mode:** Usa planificación estructurada para tareas complejas
5. **Subagentes:** Delega correcciones especializadas

## 🎯 Mejores Prácticas Avanzadas

### 1. Estructura de Proyecto Clara

```
📁 src/
├── components/     ← GitHub Copilot aprende patrones
├── services/       ← Conoce APIs
├── models/         ← Entiende estructuras de datos
└── utils/          ← Reutiliza helpers
```

### 2. Nombres Consistentes

```javascript
// ❌ Inconsistente
getUser() / fetchData() / retrieveInfo();

// ✅ Consistente
getUser() / getPosts() / getComments();
```

### 3. Comentarios Descriptivos

```python
# ❌ Poco descriptivo
def process(data):
    # process data
    pass

# ✅ Descriptivo
def calculate_monthly_revenue(sales_data: List[Sale]) -> float:
    """
    Calculate total monthly revenue from sales data.
    Applies discounts and taxes automatically.
    """
    pass
```

## 📊 Métricas de Éxito

### Cómo medir si GitHub Copilot te ayuda:

1. **Tiempo de desarrollo:** Reduce tiempo en código boilerplate
2. **Calidad:** Menos errores de sintaxis y typos
3. **Consistencia:** Código más uniforme
4. **Productividad:** Más tiempo en lógica de negocio

### Configuración Personalizada (Noviembre 2025)

Adapta GitHub Copilot a tu estilo:

```json
{
  "github.copilot.chat.customOAIModels": [],
  "chat.agent.thinkingStyle": "expanded",
  "chat.mcp.autostart": true,
  "chat.useNestedAgentsMdFiles": true,
  "chat.mcp.gallery.enabled": true
}
```

## 🔄 Actualización Continua (Noviembre 2025)

GitHub Copilot mejora constantemente con nuevas funcionalidades:

- **Modelos GPT-4.1/GPT-5-Codex:** Más precisos y optimizados para desarrollo
- **Plan Mode:** Investigación y planificación estructurada
- **Subagentes:** Procesamiento especializado en segundo plano
- **Agent Sessions:** Vista unificada para gestión de sesiones
- **MCP Marketplace:** Explorador integrado de servidores MCP
- **Code Review Mejorado:** Integración con CodeQL/ESLint y handoff directo
- **Autenticación Mejorada:** Soporte para Apple accounts y brokers nativos

### Nuevos Comandos y Funcionalidades (2025)
- `/explain` - Explica código seleccionado
- `/fix` - Corrige errores automáticamente
- `/test` - Genera tests para funciones
- `/doc` - Crea documentación
- `#runSubagent` - Delega tareas especializadas
- **Plan Mode:** Investigación estructurada antes del desarrollo

Mantente actualizado para aprovechar las mejoras.

---

_Guía optimizada para el curso "GitHub Copilot para Desarrolladores Web" - Noviembre 2025_
