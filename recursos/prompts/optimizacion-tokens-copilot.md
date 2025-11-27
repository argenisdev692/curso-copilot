# 💰 Optimización de Tokens en GitHub Copilot

> Maximiza resultados, minimiza consumo.

---

## 1. Lo Básico

| Concepto | Valor |
|----------|-------|
| 1 token | ~4 caracteres / ¾ palabra |
| Input | Barato ($) |
| Output | Caro (3-4x input) |

```
❌ 5 prompts vagos = 5x costo
✅ 1 prompt estructurado = 1x costo + mejor resultado
```

---

## 2. Principios Clave

### Precisión > Verbosidad
```
❌ "Crea un servicio en Angular que haga petición HTTP GET 
   para obtener usuarios y que devuelva observable..." (47 tokens)

✅ "Svc Angular: GET /api/users → Observable<User[]>, catchError" (15 tokens)
```

### Estructura > Prosa
```
❌ "Necesito que crees un controlador que tenga endpoints..."

✅ "ProductsController .NET 8
   CRUD | Inyectar: IProductService | Return: ApiResponse<T>"
```

### Una Tarea = Un Prompt
```
❌ "Crea modelo, DTO, servicio, repositorio, controller y tests"

✅ Prompt 1: "Entity Product: Id, Name, Price"
   Prompt 2: "DTOs: CreateProductDto, ProductResponseDto"  
   Prompt 3: "ProductService CRUD"
```

---

## 3. Técnicas Rápidas

| Técnica | Ejemplo |
|---------|---------|
| Abreviar | CRUD, DTO, DI, EF, Auth, Repo, Svc |
| Referenciar | `#file:UserService.cs` en vez de copiar |
| Compactar | `Input: string, Output: User?, Async: sí` |

---

## 4. Anti-patrones

| ❌ Evitar | Por qué |
|----------|---------|
| Prompt novela | 80% es relleno |
| Múltiples preguntas | Respuesta incompleta en todo |
| Iterar sin estructura | 4 prompts cuando 1 bastaba |
| Contexto excesivo | Pagas líneas irrelevantes |

---

## 5. Checklist

**Antes de enviar:**
- [ ] ¿Formato estructurado?
- [ ] ¿Sin palabras de relleno?
- [ ] ¿UNA sola tarea?
- [ ] ¿Usé abreviaciones?
- [ ] ¿Contexto mínimo necesario?

---

## Reglas de Oro

```
ESTRUCTURA > Prosa
PRECISIÓN > Verbosidad  
REFERENCIAS > Copiar
UNA TAREA > Múltiples
CONTEXTO MÍNIMO > Todo el archivo
```

---

> **Más valor por token invertido** > Gastar menos tokens
