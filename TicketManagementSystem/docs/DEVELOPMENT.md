# Guía de Desarrollo

## Estructura del Proyecto

```
TicketManagementSystem/
├── backend/
│   ├── TicketManagementSystem.API/
│   │   ├── Controllers/     # Endpoints API
│   │   ├── Models/         # Entidades de dominio
│   │   ├── DTOs/           # Data Transfer Objects
│   │   ├── Services/       # Lógica de negocio
│   │   ├── Repositories/   # Acceso a datos
│   │   ├── Validators/     # Validaciones
│   │   ├── Mappings/       # AutoMapper profiles
│   │   └── Helpers/        # Utilidades
├── frontend/
│   ├── ticket-system-app/
│   │   ├── src/
│   │   │   ├── app/
│   │   │   │   ├── components/  # Componentes Angular
│   │   │   │   ├── services/    # Servicios
│   │   │   │   ├── models/      # Interfaces TypeScript
│   │   │   │   ├── guards/      # Route guards
│   │   │   │   └── interceptors/# HTTP interceptors
```

## Convenciones de Código

### Backend (.NET)

#### Naming Conventions
- Clases: PascalCase (e.g., `TicketService`)
- Métodos: PascalCase (e.g., `GetByIdAsync`)
- Propiedades: PascalCase (e.g., `CreatedAt`)
- Variables locales: camelCase (e.g., `ticketId`)
- Constantes: PascalCase (e.g., `MaxTitleLength`)

#### Patrón Result
```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}
```

#### Validación
```csharp
public class CreateTicketDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public Priority Priority { get; set; }
}
```

### Frontend (Angular)

#### Componentes
```typescript
@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [CommonModule, MatTableModule],
  template: `
    @if (loading()) {
      <div>Loading...</div>
    } @else {
      <table mat-table [dataSource]="tickets()">
        <!-- table content -->
      </table>
    }
  `
})
export class TicketListComponent {
  private ticketService = inject(TicketService);

  tickets = signal<Ticket[]>([]);
  loading = signal(false);

  async ngOnInit() {
    await this.loadTickets();
  }

  private async loadTickets() {
    this.loading.set(true);
    try {
      const result = await firstValueFrom(this.ticketService.getAll());
      this.tickets.set(result.items);
    } finally {
      this.loading.set(false);
    }
  }
}
```

#### Servicios
```typescript
@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private apiUrl = inject(API_URL);

  constructor(private http: HttpClient) {}

  getAll(params?: PaginationParams): Observable<PagedResult<Ticket>> {
    return this.http.get<PagedResult<Ticket>>(`${this.apiUrl}/tickets`, {
      params: this.buildParams(params)
    });
  }

  private buildParams(params?: PaginationParams): HttpParams {
    let httpParams = new HttpParams();
    if (params?.pageNumber) {
      httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
    }
    return httpParams;
  }
}
```

## Testing

### Backend - Unit Tests
```csharp
public class TicketServiceTests
{
    private readonly Mock<ITicketRepository> _repositoryMock;
    private readonly TicketService _service;

    public TicketServiceTests()
    {
        _repositoryMock = new Mock<ITicketRepository>();
        _service = new TicketService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingTicket_ReturnsSuccess()
    {
        // Arrange
        var ticketId = 1;
        var expectedTicket = new Ticket { Id = ticketId, Title = "Test" };
        _repositoryMock.Setup(r => r.GetByIdAsync(ticketId))
            .ReturnsAsync(expectedTicket);

        // Act
        var result = await _service.GetByIdAsync(ticketId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expectedTicket);
    }
}
```

### Frontend - Unit Tests
```typescript
describe('TicketService', () => {
  let service: TicketService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TicketService],
      imports: [HttpClientTestingModule]
    });

    service = TestBed.inject(TicketService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should get all tickets', () => {
    const mockResponse: PagedResult<Ticket> = {
      items: [{ id: 1, title: 'Test Ticket' }],
      totalCount: 1,
      pageNumber: 1,
      pageSize: 10,
      totalPages: 1
    };

    service.getAll().subscribe(result => {
      expect(result).toEqual(mockResponse);
    });

    const req = httpMock.expectOne('/api/tickets');
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });
});
```

## Git Workflow

### Branch Naming
- `feature/description`: Nuevas funcionalidades
- `bugfix/description`: Corrección de bugs
- `hotfix/description`: Parches críticos
- `refactor/description`: Refactorización de código

### Commit Messages
```
type(scope): description

[optional body]

[optional footer]
```

Types: `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`

## Code Review Checklist

### Backend
- [ ] XML comments en métodos públicos
- [ ] Validación de entrada
- [ ] Manejo de errores apropiado
- [ ] Tests unitarios incluidos
- [ ] SOLID principles aplicados

### Frontend
- [ ] JSDoc en funciones exportadas
- [ ] Manejo de errores en observables
- [ ] Tests unitarios incluidos
- [ ] Accesibilidad considerada
- [ ] Performance optimizada