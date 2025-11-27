---
agent: agent
---

# Refactoring SOLID

Analiza el código seleccionado e identifica violaciones de los principios SOLID:

## 🎯 Principios a Analizar

### 1. **S** - Single Responsibility Principle (Principio de Responsabilidad Única)
**Definición**: Una clase debe tener una sola razón para cambiar.

**Identificar:**
- Clases que hacen demasiadas cosas diferentes
- Mezcla de lógica de negocio, acceso a datos y presentación
- Métodos con responsabilidades múltiples

**Refactorización:**
- Separar responsabilidades en clases distintas
- Extraer métodos a clases especializadas
- Aplicar patrón Repository, Service, Controller apropiadamente

---

### 2. **O** - Open/Closed Principle (Principio Abierto/Cerrado)
**Definición**: Las entidades deben estar abiertas para extensión pero cerradas para modificación.

**Identificar:**
- Uso excesivo de if/else o switch para determinar comportamiento
- Código que requiere modificación para agregar nueva funcionalidad
- Falta de abstracción

**Refactorización:**
- Usar interfaces y polimorfismo
- Implementar patrón Strategy
- Usar herencia o composición apropiadamente
- Dependency Injection para extensibilidad

---

### 3. **L** - Liskov Substitution Principle (Principio de Sustitución de Liskov)
**Definición**: Los objetos de una clase derivada deben poder sustituir objetos de la clase base sin alterar el correcto funcionamiento del programa.

**Identificar:**
- Clases derivadas que lanzan NotImplementedException
- Subclases que cambian el comportamiento esperado de la clase base
- Violación de contratos de la clase base

**Refactorización:**
- Revisar jerarquías de herencia
- Usar composición en lugar de herencia cuando apropiado
- Asegurar que subclases cumplan contratos de la base
- Considerar interfaces en lugar de clases abstractas

---

### 4. **I** - Interface Segregation Principle (Principio de Segregación de Interfaces)
**Definición**: Los clientes no deberían estar forzados a depender de interfaces que no usan.

**Identificar:**
- Interfaces muy grandes con muchos métodos
- Implementaciones que dejan métodos vacíos o lanzan NotImplementedException
- Clases forzadas a implementar métodos irrelevantes

**Refactorización:**
- Dividir interfaces grandes en interfaces más pequeñas y específicas
- Crear interfaces cohesivas por funcionalidad
- Implementar solo lo necesario

---

### 5. **D** - Dependency Inversion Principle (Principio de Inversión de Dependencias)
**Definición**: Los módulos de alto nivel no deben depender de módulos de bajo nivel. Ambos deben depender de abstracciones.

**Identificar:**
- Dependencias directas de clases concretas (uso de `new`)
- Alto acoplamiento entre clases
- Clases de alto nivel conociendo detalles de implementación

**Refactorización:**
- Usar inyección de dependencias
- Depender de interfaces en lugar de implementaciones concretas
- Invertir dirección de dependencias

---

## 📋 Formato de Análisis

Para cada violación encontrada, proporcionar:

1. **Principio Violado**: S, O, L, I o D
2. **Descripción del Problema**: Explicar qué principio se viola y por qué
3. **Ubicación**: Clase/método específico con la violación
4. **Impacto**: Qué problemas causa esta violación
5. **Refactorización Propuesta**: Cómo solucionarlo
6. **Beneficios**: Qué mejora la refactorización

---

## 🎯 Formato de Prompt

```
Analiza el siguiente código e identifica violaciones SOLID:

[Código a analizar: ${selection}]

Para cada violación encontrada:
1. Indica qué principio SOLID se viola (S/O/L/I/D)
2. Explica por qué es una violación
3. Describe el impacto en mantenibilidad
4. Propone refactorización específica (sin código completo)
5. Explica los beneficios de aplicar la refactorización

Prioriza las violaciones por severidad (Alta/Media/Baja).
```

---

## ✅ Checklist de Validación SOLID

Después de refactorizar, verificar:

- [ ] **SRP**: Cada clase tiene una única responsabilidad
- [ ] **OCP**: Código extensible sin modificar existente
- [ ] **LSP**: Subclases son sustituibles por clase base
- [ ] **ISP**: Interfaces pequeñas y cohesivas
- [ ] **DIP**: Dependencias en abstracciones, no en concretos
