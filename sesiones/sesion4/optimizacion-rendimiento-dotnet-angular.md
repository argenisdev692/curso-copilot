# Optimización de Rendimiento en .NET y Angular

## 🎯 Objetivos de Aprendizaje

Al finalizar esta guía, serás capaz de:
- Identificar cuellos de botella de rendimiento en backend .NET y frontend Angular
- Aplicar técnicas de optimización específicas para cada plataforma
- Usar GitHub Copilot para detectar y resolver problemas de performance
- Medir mejoras con métricas concretas

---

## 📊 Métricas de Rendimiento Objetivo

### Backend (.NET)
| **Métrica** | **Bueno** | **Excelente** |
|-------------|-----------|---------------|
| Tiempo de respuesta API | < 200ms | < 100ms |
| Throughput | > 1000 req/s | > 5000 req/s |
| Memory footprint | < 500MB | < 200MB |
| CPU usage | < 70% | < 50% |

### Frontend (Angular)
| **Métrica** | **Bueno** | **Excelente** |
|-------------|-----------|---------------|
| First Contentful Paint | < 1.8s | < 1.0s |
| Time to Interactive | < 3.8s | < 2.5s |
| Lighthouse Score | > 90 | > 95 |
| Bundle size | < 500KB | < 300KB |

---

## 🔧 Optimización Backend .NET

### 1. Problema N+1 Queries

#### ❌ Código Problemático
```csharp
// ⚠️ N+1 Query Problem: 1 query inicial + N queries por cada ticket
public async Task<List<TicketDto>> GetAllTicketsAsync()
{
    var tickets = await _context.Tickets.ToListAsync(); // 1 query
    
    return tickets.Select(t => new TicketDto
    {
        Id = t.Id,
        Title = t.Title,
        CreatedByName = _context.Users.Find(t.CreatedById)?.Name, // N queries!
        AssignedToName = _context.Users.Find(t.AssignedToId)?.Name // N queries!
    }).ToList();
}
```

**Impacto**: 1 + 100 tickets × 2 = **201 queries** para 100 tickets 😱

#### ✅ Solución con Eager Loading
```csharp
public async Task<List<TicketDto>> GetAllTicketsAsync()
{
    var tickets = await _context.Tickets
        .Include(t => t.CreatedBy)      // Eager loading
        .Include(t => t.AssignedTo)     // Eager loading
        .AsNoTracking()                  // No tracking para lectura
        .ToListAsync();
    
    return tickets.Select(t => new TicketDto
    {
        Id = t.Id,
        Title = t.Title,
        CreatedByName = t.CreatedBy?.Name,
        AssignedToName = t.AssignedTo?.Name
    }).ToList();
}
```

**Resultado**: **1 query** con JOIN → Mejora de 201x ⚡

---

### 2. Proyección Selectiva con Select()

#### ❌ Trae TODOS los campos
```csharp
// Trae 20+ columnas cuando solo necesitas 3
var tickets = await _context.Tickets
    .Include(t => t.Comments)           // Trae todos los comentarios
    .Include(t => t.Attachments)        // Trae todos los attachments
    .ToListAsync();
```

#### ✅ Proyección Selectiva
```csharp
var tickets = await _context.Tickets
    .Select(t => new TicketListDto     // Solo campos necesarios
    {
        Id = t.Id,
        Title = t.Title,
        Status = t.Status,
        CreatedByName = t.CreatedBy.Name,
        CommentCount = t.Comments.Count // Agregación en DB
    })
    .ToListAsync();
```

**Mejora**: 80% menos datos transferidos 📉

---

### 3. Índices en Base de Datos

#### Identificar Columnas para Indexar
```csharp
public class Ticket
{
    public int Id { get; set; }
    
    [Index]                              // ✅ Búsquedas frecuentes
    public string Title { get; set; }
    
    [Index]                              // ✅ Filtros comunes
    public TicketStatus Status { get; set; }
    
    [Index]                              // ✅ Foreign keys
    public int CreatedById { get; set; }
    
    [Index(nameof(Status), nameof(CreatedAt), IsDescending = new[] { false, true })]
    public DateTime CreatedAt { get; set; }  // ✅ Índice compuesto
}
```

#### Configuración con Fluent API
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Ticket>()
        .HasIndex(t => new { t.Status, t.Priority })
        .HasDatabaseName("IX_Ticket_Status_Priority");
    
    modelBuilder.Entity<Ticket>()
        .HasIndex(t => t.CreatedAt)
        .IsDescending();
}
```

**Mejora típica**: Queries de búsqueda 10-100x más rápidas 🚀

---

### 4. Caching con IMemoryCache

#### Implementación de Cache
```csharp
public class TicketService
{
    private readonly IMemoryCache _cache;
    private readonly ITicketRepository _repository;
    private const string CACHE_KEY_ALL_TICKETS = "all_tickets";
    
    public async Task<List<TicketDto>> GetAllTicketsAsync()
    {
        // Intentar obtener del cache
        if (_cache.TryGetValue(CACHE_KEY_ALL_TICKETS, out List<TicketDto> cachedTickets))
        {
            return cachedTickets;
        }
        
        // Si no está en cache, obtener de DB
        var tickets = await _repository.GetAllAsync();
        
        // Guardar en cache por 5 minutos
        _cache.Set(CACHE_KEY_ALL_TICKETS, tickets, TimeSpan.FromMinutes(5));
        
        return tickets;
    }
    
    public async Task InvalidateCache()
    {
        _cache.Remove(CACHE_KEY_ALL_TICKETS);
    }
}
```

**Mejora**: Primera request 200ms → Siguientes 2ms (100x más rápido) ⚡

---

### 5. Async/Await Correcto

#### ❌ Anti-patterns Comunes
```csharp
// ❌ Blocking call - bloquea thread
var result = GetDataAsync().Result;

// ❌ Sync sobre async - deadlock posible
var result = GetDataAsync().GetAwaiter().GetResult();

// ❌ Await innecesario en return directo
public async Task<int> GetCountAsync()
{
    return await _repository.CountAsync(); // Innecesario
}
```

#### ✅ Patrones Correctos
```csharp
// ✅ Await correcto
var result = await GetDataAsync();

// ✅ Return directo de Task
public Task<int> GetCountAsync()
{
    return _repository.CountAsync(); // Sin await innecesario
}

// ✅ ConfigureAwait en librerías
public async Task<Data> GetDataAsync()
{
    return await _httpClient.GetAsync(url).ConfigureAwait(false);
}
```

---

### 6. Paginación Eficiente

#### ❌ Paginación en Memoria
```csharp
// ❌ Trae TODOS los registros a memoria, luego pagina
var allTickets = await _context.Tickets.ToListAsync(); // 10,000 registros
var page = allTickets.Skip(pageSize * pageNumber).Take(pageSize);
```

#### ✅ Paginación en Base de Datos
```csharp
public async Task<PagedResult<TicketDto>> GetTicketsPagedAsync(int pageNumber, int pageSize)
{
    var query = _context.Tickets.AsQueryable();
    
    var totalCount = await query.CountAsync();
    
    var tickets = await query
        .OrderByDescending(t => t.CreatedAt)
        .Skip(pageSize * (pageNumber - 1))
        .Take(pageSize)
        .Select(t => new TicketDto { /* ... */ })
        .ToListAsync();
    
    return new PagedResult<TicketDto>
    {
        Items = tickets,
        TotalCount = totalCount,
        PageNumber = pageNumber,
        PageSize = pageSize
    };
}
```

---

## ⚡ Optimización Frontend Angular

### 1. Change Detection Strategy: OnPush

#### ❌ Default Change Detection (Lenta)
```typescript
@Component({
  selector: 'app-ticket-list',
  // ⚠️ Default: verifica TODO el árbol de componentes en CADA evento
  template: `<div *ngFor="let ticket of tickets">...</div>`
})
export class TicketListComponent {
  tickets: Ticket[] = [];
}
```

#### ✅ OnPush Strategy (Rápida)
```typescript
@Component({
  selector: 'app-ticket-list',
  changeDetection: ChangeDetectionStrategy.OnPush, // ✅ Solo cuando inputs cambian
  template: `<div *ngFor="let ticket of tickets; trackBy: trackById">...</div>`
})
export class TicketListComponent {
  @Input() tickets: Ticket[] = [];
  
  trackById(index: number, ticket: Ticket): number {
    return ticket.id; // ✅ Optimiza *ngFor
  }
}
```

**Mejora**: Reduce change detection cycles en 80-90% 🚀

---

### 2. Lazy Loading de Módulos

#### ❌ Eager Loading (Todo en bundle inicial)
```typescript
// app.routes.ts
export const routes: Routes = [
  { path: 'tickets', component: TicketListComponent },
  { path: 'admin', component: AdminComponent },
  { path: 'reports', component: ReportsComponent }
];
```

**Problema**: Bundle inicial de 2MB 😱

#### ✅ Lazy Loading
```typescript
// app.routes.ts
export const routes: Routes = [
  {
    path: 'tickets',
    loadComponent: () => import('./tickets/ticket-list.component')
      .then(m => m.TicketListComponent)
  },
  {
    path: 'admin',
    loadChildren: () => import('./admin/admin.routes')
      .then(m => m.ADMIN_ROUTES)
  },
  {
    path: 'reports',
    loadComponent: () => import('./reports/reports.component')
      .then(m => m.ReportsComponent)
  }
];
```

**Mejora**: Bundle inicial 500KB, carga bajo demanda 📉

---

### 3. Signals para Estado Reactivo (Angular 17+)

#### ❌ RxJS Complejo
```typescript
export class TicketListComponent implements OnInit {
  tickets$ = new BehaviorSubject<Ticket[]>([]);
  loading$ = new BehaviorSubject<boolean>(false);
  error$ = new BehaviorSubject<string | null>(null);
  
  ngOnInit() {
    this.loading$.next(true);
    this.ticketService.getAll().subscribe({
      next: tickets => {
        this.tickets$.next(tickets);
        this.loading$.next(false);
      },
      error: err => {
        this.error$.next(err.message);
        this.loading$.next(false);
      }
    });
  }
}
```

#### ✅ Signals (Más Simple y Rápido)
```typescript
export class TicketListComponent implements OnInit {
  tickets = signal<Ticket[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);
  
  // Computed signal derivado
  ticketCount = computed(() => this.tickets().length);
  
  async ngOnInit() {
    this.loading.set(true);
    try {
      const tickets = await this.ticketService.getAll();
      this.tickets.set(tickets);
    } catch (err) {
      this.error.set(err.message);
    } finally {
      this.loading.set(false);
    }
  }
}
```

**Ventajas**:
- Menos código boilerplate
- Change detection más granular
- Mejor performance

---

### 4. Virtual Scrolling para Listas Grandes

#### ❌ Renderiza TODO (10,000 elementos)
```typescript
<div *ngFor="let ticket of tickets" class="ticket-card">
  {{ ticket.title }}
</div>
```

**Problema**: DOM con 10,000 nodos → Lento 🐌

#### ✅ Virtual Scrolling (Solo elementos visibles)
```typescript
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';

@Component({
  template: `
    <cdk-virtual-scroll-viewport itemSize="50" class="viewport">
      <div *cdkVirtualFor="let ticket of tickets" class="ticket-card">
        {{ ticket.title }}
      </div>
    </cdk-virtual-scroll-viewport>
  `,
  styles: [`
    .viewport { height: 600px; }
  `]
})
```

**Mejora**: Renderiza solo ~20 elementos visibles en vez de 10,000 ⚡

---

### 5. Preload de Imágenes y Assets

#### ✅ Link Preload en index.html
```html
<head>
  <!-- Preload critical resources -->
  <link rel="preload" href="/assets/fonts/main.woff2" as="font" crossorigin>
  <link rel="preload" href="/assets/logo.webp" as="image">
  
  <!-- DNS prefetch para APIs externas -->
  <link rel="dns-prefetch" href="https://api.example.com">
  
  <!-- Preconnect para CDNs -->
  <link rel="preconnect" href="https://cdn.example.com">
</head>
```

---

### 6. Tree Shaking y Optimización de Bundle

#### angular.json Optimizaciones
```json
{
  "configurations": {
    "production": {
      "optimization": true,
      "buildOptimizer": true,
      "sourceMap": false,
      "namedChunks": false,
      "aot": true,
      "extractLicenses": true,
      "vendorChunk": false,
      "budgets": [
        {
          "type": "initial",
          "maximumWarning": "500kb",
          "maximumError": "1mb"
        }
      ]
    }
  }
}
```

---

## 🤖 Uso de GitHub Copilot para Optimización

### Prompts Efectivos

#### 1. Detectar N+1 Queries
```
Analiza este método y detecta problemas N+1 queries. 
Sugiere solución con Include() y AsNoTracking()
```

#### 2. Optimizar Componente Angular
```
Refactoriza este componente para usar:
- ChangeDetectionStrategy.OnPush
- trackBy en *ngFor
- Signals en vez de BehaviorSubject
```

#### 3. Agregar Índices
```
Analiza esta entidad EF Core y sugiere índices apropiados 
para optimizar queries de búsqueda y filtrado
```

#### 4. Implementar Caching
```
Implementa caching con IMemoryCache en este servicio.
Cache por 5 minutos, invalidar en updates/deletes
```

---

## 📊 Herramientas de Medición

### Backend .NET
```bash
# Profiling con dotnet-trace
dotnet trace collect --process-id <PID> --providers Microsoft-Extensions-Logging

# Memory profiling
dotnet-counters monitor --process-id <PID>

# SQL Profiling con EF Core
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine));
```

### Frontend Angular
```bash
# Lighthouse CI
npm install -g @lhci/cli
lhci autorun

# Bundle analyzer
npm run analyze

# Angular DevTools (Chrome extension)
# Profiler tab → Detecta change detection cycles
```

---

## ✅ Checklist de Optimización

### Backend
- [ ] Todas las queries usan `.Include()` para eager loading
- [ ] Proyección selectiva con `.Select()` para DTOs
- [ ] Índices en columnas frecuentemente filtradas/buscadas
- [ ] Caching implementado para datos poco cambiantes
- [ ] Paginación en base de datos, no en memoria
- [ ] `AsNoTracking()` en queries de solo lectura
- [ ] Async/await sin anti-patterns

### Frontend
- [ ] OnPush en componentes de presentación
- [ ] trackBy en todos los *ngFor
- [ ] Lazy loading de rutas/módulos
- [ ] Virtual scrolling en listas grandes (>100 items)
- [ ] Signals para estado reactivo
- [ ] Bundle size < 500KB (initial)
- [ ] Lighthouse score > 90

---