# 💰 Tablas de Precios de Modelos de IA (Actualizado)

**Fecha de Investigación:** 2025-11-25

---

## 🎯 Diferencias Clave: Modelos "Pro/Full" vs "Mini/Flash"

### ¿Por qué existen diferentes tamaños?

| Aspecto | Pro/Full (GPT-5, Sonnet, Pro) | Mini/Flash (GPT-5 Mini, Haiku, Flash) |
|---------|-------------------------------|---------------------------------------|
| **Propósito** | Razonamiento profundo, tareas complejas | Velocidad, alto volumen, costo bajo |
| **Latencia** | 500-2000ms | 100-300ms (2-3x más rápido) |
| **Costo** | $$$ | $ (3-10x más barato) |
| **Calidad** | Máxima | 70-85% del modelo grande |
| **Uso ideal** | Análisis, código complejo, investigación | Chatbots, clasificación, tareas simples |

### Regla de Decisión Rápida

```
┌─────────────────────────────────────────────────────────────┐
│  ¿Necesitas razonamiento profundo o precisión crítica?     │
│     SÍ → Usa modelo Pro/Full                               │
│     NO → Usa modelo Mini/Flash (ahorra 70-90%)             │
└─────────────────────────────────────────────────────────────┘
```

---

## 📊 OpenAI GPT Series (Agosto 2025)

### Características por Modelo

| Modelo | Contexto | Propósito | Knowledge Cutoff |
|--------|----------|-----------|------------------|
| **GPT-5** | 400K tokens | Flagship: razonamiento profundo, código complejo | Sep 2024 |
| **GPT-5 Mini** | 400K tokens | Balance calidad/costo, tareas bien definidas | May 2024 |
| **GPT-5 Nano** | 400K tokens | Ultra bajo costo, clasificación, tagging | May 2024 |
| GPT-4o | 128K tokens | Modelo anterior, estable y capaz | - |
| GPT-4o Mini | 128K tokens | Opción económica legacy | - |

### Precios API (USD por millón de tokens)

| Modelo | Input | Output | Cuándo Usar |
|--------|-------|--------|-------------|
| **GPT-5** | $1.25 | $10.00 | Tareas complejas, razonamiento, código crítico |
| **GPT-5 Mini** | $0.25 | $2.00 | Default para la mayoría de tareas |
| **GPT-5 Nano** | $0.05 | $0.40 | Alto volumen, clasificación, enriquecimiento |
| GPT-4o | $2.50 | $10.00 | Si necesitas el modelo anterior |
| GPT-4o Mini | $0.15 | $0.60 | Tareas simples, legacy |

### GPT-5 vs GPT-5 Mini: Diferencias Clave

| Aspecto | GPT-5 | GPT-5 Mini |
|---------|-------|------------|
| **Razonamiento** | Máximo, configurable (low/medium/high) | Bueno, suficiente para mayoría de casos |
| **Tool Use** | El más capaz | Muy capaz |
| **Velocidad** | Más lento | 2-3x más rápido |
| **Costo** | 5x más caro | Fracción del costo |
| **Ideal para** | Investigación, legal, científico, código crítico | Chatbots, asistentes, tareas definidas |

> **Nota**: GPT-5 usa un "router inteligente" que decide automáticamente cuándo pensar más profundo. Cuando llegas a tu límite de uso, cambia automáticamente a GPT-5 Mini.

---

## 🟠 Anthropic Claude Series (Octubre 2025)

### Características por Modelo

| Modelo | Contexto | Propósito | Lanzamiento |
|--------|----------|-----------|-------------|
| **Claude Sonnet 4.5** | 200K (1M beta) | Mejor para coding y agentes complejos | Sep 2025 |
| **Claude Haiku 4.5** | 200K | Velocidad + inteligencia near-frontier | Oct 2025 |
| **Claude Opus 4.1** | 200K | Razonamiento especializado máximo | Ago 2025 |

### Precios API (USD por millón de tokens)

| Modelo | Input | Output | Cuándo Usar |
|--------|-------|--------|-------------|
| **Claude Sonnet 4.5** | $3.00 | $15.00 | Coding, agentes, tareas complejas |
| **Claude Haiku 4.5** | $1.00 | $5.00 | Alto volumen con buena calidad |
| **Claude Opus 4.1** | $15.00 | $75.00 | Razonamiento crítico especializado |
| Claude 3.5 Sonnet | $3.00 | $15.00 | Legacy, sigue siendo muy capaz |
| Claude 3 Haiku | $0.25 | $1.25 | Opción más económica |

### Claude Sonnet vs Haiku: Diferencias Clave

| Aspecto | Sonnet 4.5 | Haiku 4.5 |
|---------|------------|-----------|
| **Velocidad** | 500-800ms | Sub-200ms (2-3x más rápido) |
| **Razonamiento** | Avanzado | Medio-Alto |
| **Costo promedio** | ~$18/1M tokens | ~$6/1M tokens |
| **Extended Thinking** | Sí | Sí (nuevo en 4.5) |
| **Ideal para** | Código, análisis profundo | Chatbots, real-time, alto volumen |

> **Insight**: Haiku 4.5 tiene capacidades similares a Sonnet 4 (de hace 2 meses) pero a 1/3 del costo y 2-3x más rápido.

---

## 🔵 Google Gemini Series (Noviembre 2025)

### Características por Modelo

| Modelo | Contexto | Propósito | Estado |
|--------|----------|-----------|--------|
| **Gemini 3 Pro** | 2M+ tokens | Deep Think, razonamiento máximo | Nuevo (Nov 2025) |
| **Gemini 3 Flash** | 1M tokens | Latencia mínima, eficiencia | Nuevo (Nov 2025) |
| Gemini 2.5 Pro | 1M tokens | Thinking model, muy capaz | Estable |
| Gemini 2.5 Flash | 1M tokens | Balance velocidad/calidad | Estable |
| Gemini 2.5 Flash-Lite | 1M tokens | Ultra económico | Estable |

### Precios API (USD por millón de tokens)

| Modelo | Input | Output | Cuándo Usar |
|--------|-------|--------|-------------|
| **Gemini 3 Pro** | $7.50 | $30.00 | Tareas ultra complejas, agentes |
| **Gemini 3 Flash** | $0.75 | $3.00 | Real-time con buena calidad |
| Gemini 2.5 Pro | $1.25 | $5.00 | Gama media-alta, muy capaz |
| Gemini 2.5 Flash | $0.15 | $0.60 | Alto volumen económico |
| Gemini 2.5 Flash-Lite | $0.10 | $0.40 | **La opción más barata del mercado** |

### Gemini Pro vs Flash: Diferencias Clave

| Aspecto | Pro | Flash |
|---------|-----|-------|
| **Optimizado para** | Máxima calidad, razonamiento complejo | Velocidad, baja latencia |
| **Costo** | ~15x más caro que Flash | Muy económico |
| **Thinking Budgets** | Sí (configurable) | Sí (configurable) |
| **Ideal para** | Investigación, código complejo | Chatbots, routing, clasificación |

> **Feature único**: Gemini permite configurar "thinking budgets" - controlar cuántos tokens usa para "pensar" antes de responder.

---

## 💻 GitHub Copilot & Microsoft (Nov 2025)

| Producto | Modelos Disponibles | Precio | Notas |
|----------|---------------------|--------|-------|
| **GitHub Copilot Individual** | GPT-5, Claude 3.5 Sonnet, Gemini 2.5 | $10/mes | Model switching disponible |
| **GitHub Copilot Business** | Todos + enterprise features | $19/usuario/mes | Políticas, seguridad |
| **Microsoft Copilot Pro** | GPT-5 prioritario | $20/mes | Integración M365 |
| **Microsoft Copilot Free** | GPT-4o / GPT-5 Mini | Gratis | Uso limitado |

---

## 📈 Comparación de Costos por Tarea

### Tarea Simple (100 input + 200 output tokens)

| Modelo | Costo | Velocidad |
|--------|-------|-----------|
| Gemini 2.5 Flash-Lite | $0.00009 | ⚡⚡⚡ |
| GPT-5 Nano | $0.00013 | ⚡⚡⚡ |
| GPT-4o Mini | $0.00014 | ⚡⚡ |
| Claude Haiku 4.5 | $0.0011 | ⚡⚡⚡ |

### Tarea Compleja (1000 input + 1000 output tokens)

| Modelo | Costo | Calidad |
|--------|-------|---------|
| GPT-5 Mini | $0.00225 | ⭐⭐⭐⭐ |
| Claude Haiku 4.5 | $0.006 | ⭐⭐⭐⭐ |
| GPT-5 | $0.01125 | ⭐⭐⭐⭐⭐ |
| Claude Sonnet 4.5 | $0.018 | ⭐⭐⭐⭐⭐ |
| Gemini 3 Pro | $0.0375 | ⭐⭐⭐⭐⭐ |

---

## 🎯 Guía de Selección Rápida

```
┌─────────────────────────────────────────────────────────────┐
│  ¿QUÉ MODELO USAR?                                         │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  💰 Más barato posible      → Gemini 2.5 Flash-Lite        │
│  ⚡ Velocidad + calidad      → GPT-5 Mini / Haiku 4.5      │
│  🧠 Razonamiento máximo      → GPT-5 / Gemini 3 Pro        │
│  💻 Coding especializado     → Claude Sonnet 4.5           │
│  📚 Contexto gigante         → Gemini (hasta 2M tokens)    │
│  🏢 Alto volumen empresa     → GPT-5 Mini + Haiku 4.5      │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## 📌 Conclusiones (Noviembre 2025)

1. **Modelos Mini/Flash son suficientes para el 80% de casos** - No pagues por Pro si no lo necesitas.

2. **La frontera baja rápido** - Haiku 4.5 hoy = Sonnet 4 de hace 2 meses, a 1/3 del precio.

3. **GPT-5 Mini es el nuevo default** - Balance óptimo calidad/costo para la mayoría.

4. **Contexto gigante en Gemini** - Único con 1-2M tokens nativos.

5. **GitHub Copilot ya no es solo OpenAI** - Puedes elegir Claude o Gemini.

---

*Precios sujetos a cambios. Última actualización: 2025-11-25*