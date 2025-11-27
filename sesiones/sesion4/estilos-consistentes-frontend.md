# Creación de Estilos Consistentes en Frontend

## 🎯 Objetivos de Aprendizaje

Al finalizar esta guía, serás capaz de:
- Establecer una guía de estilos CSS/SCSS coherente y escalable
- Aplicar metodologías de naming (BEM, SMACSS)
- Configurar Tailwind CSS con design system personalizado
- Usar GitHub Copilot para generar estilos consistentes
- Implementar variables CSS y tokens de diseño

---

## 🎨 Fundamentos de Design System

### Anatomía de un Design System

```
Design System
├── Design Tokens (variables)
│   ├── Colors
│   ├── Typography
│   ├── Spacing
│   └── Shadows
├── Components (patrones reutilizables)
│   ├── Buttons
│   ├── Cards
│   ├── Forms
│   └── Navigation
└── Patterns (composición de components)
    ├── Page Layouts
    ├── Modal Dialogs
    └── Data Tables
```

---

## 🔧 Configuración Inicial

### 1. Variables CSS (Custom Properties)

#### `styles/variables.scss`
```scss
:root {
  // Colors - Primary Palette
  --color-primary-50: #f0f9ff;
  --color-primary-100: #e0f2fe;
  --color-primary-200: #bae6fd;
  --color-primary-300: #7dd3fc;
  --color-primary-400: #38bdf8;
  --color-primary-500: #0ea5e9;  // Main brand color
  --color-primary-600: #0284c7;
  --color-primary-700: #0369a1;
  --color-primary-800: #075985;
  --color-primary-900: #0c4a6e;
  
  // Colors - Neutral Palette
  --color-neutral-50: #fafafa;
  --color-neutral-100: #f5f5f5;
  --color-neutral-200: #e5e5e5;
  --color-neutral-300: #d4d4d4;
  --color-neutral-400: #a3a3a3;
  --color-neutral-500: #737373;
  --color-neutral-600: #525252;
  --color-neutral-700: #404040;
  --color-neutral-800: #262626;
  --color-neutral-900: #171717;
  
  // Colors - Semantic
  --color-success: #10b981;
  --color-warning: #f59e0b;
  --color-error: #ef4444;
  --color-info: #3b82f6;
  
  // Typography
  --font-family-sans: 'Inter', system-ui, -apple-system, sans-serif;
  --font-family-mono: 'Fira Code', 'Courier New', monospace;
  
  --font-size-xs: 0.75rem;    // 12px
  --font-size-sm: 0.875rem;   // 14px
  --font-size-base: 1rem;     // 16px
  --font-size-lg: 1.125rem;   // 18px
  --font-size-xl: 1.25rem;    // 20px
  --font-size-2xl: 1.5rem;    // 24px
  --font-size-3xl: 1.875rem;  // 30px
  --font-size-4xl: 2.25rem;   // 36px
  
  --font-weight-normal: 400;
  --font-weight-medium: 500;
  --font-weight-semibold: 600;
  --font-weight-bold: 700;
  
  --line-height-tight: 1.25;
  --line-height-normal: 1.5;
  --line-height-relaxed: 1.75;
  
  // Spacing (8px base grid)
  --spacing-1: 0.25rem;   // 4px
  --spacing-2: 0.5rem;    // 8px
  --spacing-3: 0.75rem;   // 12px
  --spacing-4: 1rem;      // 16px
  --spacing-5: 1.25rem;   // 20px
  --spacing-6: 1.5rem;    // 24px
  --spacing-8: 2rem;      // 32px
  --spacing-10: 2.5rem;   // 40px
  --spacing-12: 3rem;     // 48px
  --spacing-16: 4rem;     // 64px
  
  // Border Radius
  --radius-sm: 0.25rem;   // 4px
  --radius-md: 0.375rem;  // 6px
  --radius-lg: 0.5rem;    // 8px
  --radius-xl: 0.75rem;   // 12px
  --radius-2xl: 1rem;     // 16px
  --radius-full: 9999px;  // Circular
  
  // Shadows
  --shadow-sm: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
  --shadow-md: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
  --shadow-lg: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
  --shadow-xl: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
  
  // Transitions
  --transition-fast: 150ms ease-in-out;
  --transition-base: 200ms ease-in-out;
  --transition-slow: 300ms ease-in-out;
  
  // Z-Index layers
  --z-dropdown: 1000;
  --z-sticky: 1020;
  --z-fixed: 1030;
  --z-modal-backdrop: 1040;
  --z-modal: 1050;
  --z-popover: 1060;
  --z-tooltip: 1070;
}
```

---

## 📐 Metodología BEM (Block Element Modifier)

### Estructura de Naming

```
.block {}
.block__element {}
.block--modifier {}
.block__element--modifier {}
```

### Ejemplo: Componente Card

#### ❌ Naming Inconsistente
```scss
// Mal: Nombres ambiguos, anidamiento profundo
.card {
  .header {
    .title {
      .icon {}
    }
  }
  .content {}
  .actions {
    .button {}
    .button.primary {}
  }
}
```

#### ✅ BEM Correcto
```scss
// Block
.card {
  background: var(--color-neutral-50);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-md);
  padding: var(--spacing-6);
}

// Elements
.card__header {
  border-bottom: 1px solid var(--color-neutral-200);
  padding-bottom: var(--spacing-4);
  margin-bottom: var(--spacing-4);
}

.card__title {
  font-size: var(--font-size-xl);
  font-weight: var(--font-weight-semibold);
  color: var(--color-neutral-900);
  display: flex;
  align-items: center;
  gap: var(--spacing-2);
}

.card__title-icon {
  width: 24px;
  height: 24px;
  color: var(--color-primary-500);
}

.card__content {
  font-size: var(--font-size-base);
  line-height: var(--line-height-relaxed);
  color: var(--color-neutral-700);
}

.card__actions {
  display: flex;
  gap: var(--spacing-3);
  margin-top: var(--spacing-6);
}

// Modifiers
.card--highlighted {
  border: 2px solid var(--color-primary-500);
  box-shadow: var(--shadow-lg);
}

.card--compact {
  padding: var(--spacing-4);
}

.card--error {
  border-left: 4px solid var(--color-error);
  background: #fef2f2;
}
```

#### Uso en HTML
```html
<div class="card card--highlighted">
  <div class="card__header">
    <h3 class="card__title">
      <svg class="card__title-icon">...</svg>
      Ticket #123
    </h3>
  </div>
  <div class="card__content">
    Descripción del ticket...
  </div>
  <div class="card__actions">
    <button class="btn btn--primary">Editar</button>
    <button class="btn btn--secondary">Cerrar</button>
  </div>
</div>
```

---

## 🎨 Tailwind CSS + Design Tokens

### Configuración `tailwind.config.js`

```javascript
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#f0f9ff',
          100: '#e0f2fe',
          200: '#bae6fd',
          300: '#7dd3fc',
          400: '#38bdf8',
          500: '#0ea5e9',  // Main
          600: '#0284c7',
          700: '#0369a1',
          800: '#075985',
          900: '#0c4a6e',
        },
        neutral: {
          50: '#fafafa',
          100: '#f5f5f5',
          200: '#e5e5e5',
          300: '#d4d4d4',
          400: '#a3a3a3',
          500: '#737373',
          600: '#525252',
          700: '#404040',
          800: '#262626',
          900: '#171717',
        },
        success: '#10b981',
        warning: '#f59e0b',
        error: '#ef4444',
        info: '#3b82f6',
      },
      fontFamily: {
        sans: ['Inter', 'system-ui', 'sans-serif'],
        mono: ['Fira Code', 'Courier New', 'monospace'],
      },
      fontSize: {
        xs: ['0.75rem', { lineHeight: '1rem' }],
        sm: ['0.875rem', { lineHeight: '1.25rem' }],
        base: ['1rem', { lineHeight: '1.5rem' }],
        lg: ['1.125rem', { lineHeight: '1.75rem' }],
        xl: ['1.25rem', { lineHeight: '1.75rem' }],
        '2xl': ['1.5rem', { lineHeight: '2rem' }],
        '3xl': ['1.875rem', { lineHeight: '2.25rem' }],
        '4xl': ['2.25rem', { lineHeight: '2.5rem' }],
      },
      spacing: {
        '1': '0.25rem',
        '2': '0.5rem',
        '3': '0.75rem',
        '4': '1rem',
        '5': '1.25rem',
        '6': '1.5rem',
        '8': '2rem',
        '10': '2.5rem',
        '12': '3rem',
        '16': '4rem',
      },
      borderRadius: {
        sm: '0.25rem',
        md: '0.375rem',
        lg: '0.5rem',
        xl: '0.75rem',
        '2xl': '1rem',
        full: '9999px',
      },
      boxShadow: {
        sm: '0 1px 2px 0 rgba(0, 0, 0, 0.05)',
        md: '0 4px 6px -1px rgba(0, 0, 0, 0.1)',
        lg: '0 10px 15px -3px rgba(0, 0, 0, 0.1)',
        xl: '0 20px 25px -5px rgba(0, 0, 0, 0.1)',
      },
      transitionDuration: {
        fast: '150ms',
        base: '200ms',
        slow: '300ms',
      },
    },
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography'),
  ],
};
```

---

## 🧩 Componentes Reutilizables

### 1. Sistema de Botones

#### `components/button.scss`
```scss
.btn {
  // Base styles
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: var(--spacing-2);
  padding: var(--spacing-3) var(--spacing-6);
  font-size: var(--font-size-base);
  font-weight: var(--font-weight-medium);
  border-radius: var(--radius-md);
  border: 1px solid transparent;
  cursor: pointer;
  transition: all var(--transition-base);
  
  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }
  
  // Variants
  &--primary {
    background: var(--color-primary-500);
    color: white;
    
    &:hover:not(:disabled) {
      background: var(--color-primary-600);
      box-shadow: var(--shadow-md);
    }
    
    &:active:not(:disabled) {
      background: var(--color-primary-700);
    }
  }
  
  &--secondary {
    background: transparent;
    color: var(--color-primary-500);
    border-color: var(--color-primary-500);
    
    &:hover:not(:disabled) {
      background: var(--color-primary-50);
    }
  }
  
  &--danger {
    background: var(--color-error);
    color: white;
    
    &:hover:not(:disabled) {
      background: #dc2626;
    }
  }
  
  &--ghost {
    background: transparent;
    color: var(--color-neutral-700);
    
    &:hover:not(:disabled) {
      background: var(--color-neutral-100);
    }
  }
  
  // Sizes
  &--sm {
    padding: var(--spacing-2) var(--spacing-4);
    font-size: var(--font-size-sm);
  }
  
  &--lg {
    padding: var(--spacing-4) var(--spacing-8);
    font-size: var(--font-size-lg);
  }
  
  // Full width
  &--block {
    width: 100%;
  }
  
  // Icon only
  &--icon {
    padding: var(--spacing-3);
    aspect-ratio: 1;
  }
}
```

#### Uso en Angular
```typescript
@Component({
  selector: 'app-button',
  template: `
    <button 
      [class]="buttonClasses"
      [disabled]="disabled"
      [type]="type"
      (click)="onClick.emit($event)">
      <ng-content></ng-content>
    </button>
  `,
  styleUrls: ['./button.component.scss']
})
export class ButtonComponent {
  @Input() variant: 'primary' | 'secondary' | 'danger' | 'ghost' = 'primary';
  @Input() size: 'sm' | 'md' | 'lg' = 'md';
  @Input() block = false;
  @Input() disabled = false;
  @Input() type: 'button' | 'submit' = 'button';
  @Output() onClick = new EventEmitter<MouseEvent>();
  
  get buttonClasses(): string {
    return [
      'btn',
      `btn--${this.variant}`,
      this.size !== 'md' ? `btn--${this.size}` : '',
      this.block ? 'btn--block' : ''
    ].filter(Boolean).join(' ');
  }
}
```

```html
<!-- Uso -->
<app-button variant="primary" size="lg" (onClick)="save()">
  Guardar
</app-button>

<app-button variant="secondary" size="sm">
  Cancelar
</app-button>

<app-button variant="danger" [disabled]="loading">
  Eliminar
</app-button>
```

---

### 2. Sistema de Forms

#### `components/form-field.scss`
```scss
.form-field {
  display: flex;
  flex-direction: column;
  gap: var(--spacing-2);
  margin-bottom: var(--spacing-6);
  
  &__label {
    font-size: var(--font-size-sm);
    font-weight: var(--font-weight-medium);
    color: var(--color-neutral-700);
    
    &--required::after {
      content: '*';
      color: var(--color-error);
      margin-left: var(--spacing-1);
    }
  }
  
  &__input {
    padding: var(--spacing-3) var(--spacing-4);
    font-size: var(--font-size-base);
    border: 1px solid var(--color-neutral-300);
    border-radius: var(--radius-md);
    transition: all var(--transition-fast);
    
    &:focus {
      outline: none;
      border-color: var(--color-primary-500);
      box-shadow: 0 0 0 3px rgba(14, 165, 233, 0.1);
    }
    
    &:disabled {
      background: var(--color-neutral-100);
      cursor: not-allowed;
    }
    
    &--error {
      border-color: var(--color-error);
      
      &:focus {
        box-shadow: 0 0 0 3px rgba(239, 68, 68, 0.1);
      }
    }
  }
  
  &__hint {
    font-size: var(--font-size-xs);
    color: var(--color-neutral-500);
  }
  
  &__error {
    font-size: var(--font-size-xs);
    color: var(--color-error);
    display: flex;
    align-items: center;
    gap: var(--spacing-1);
  }
}
```

---

## 📱 Responsive Design Patterns

### Mobile-First Approach
```scss
// Base: Mobile (< 640px)
.container {
  padding: var(--spacing-4);
}

.grid {
  display: grid;
  grid-template-columns: 1fr;
  gap: var(--spacing-4);
}

// Tablet (≥ 768px)
@media (min-width: 768px) {
  .container {
    padding: var(--spacing-6);
  }
  
  .grid {
    grid-template-columns: repeat(2, 1fr);
    gap: var(--spacing-6);
  }
}

// Desktop (≥ 1024px)
@media (min-width: 1024px) {
  .container {
    max-width: 1200px;
    margin: 0 auto;
    padding: var(--spacing-8);
  }
  
  .grid {
    grid-template-columns: repeat(3, 1fr);
  }
}
```

### Tailwind Responsive Classes
```html
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 md:gap-6">
  <div class="p-4 md:p-6 lg:p-8">...</div>
</div>
```

---

## 🤖 Uso de GitHub Copilot para Estilos

### Prompts Efectivos

#### 1. Generar Componente con Estilos BEM
```
Crea un componente Angular "alert" con variantes success, warning, error, info.
Usa metodología BEM y variables CSS. Incluye icono y botón de cerrar.
```

#### 2. Generar Design Tokens
```
Genera un archivo de variables CSS con:
- Paleta de colores con 9 shades (50-900)
- Typography scale (12px a 48px)
- Spacing scale basado en grid de 8px
- Border radius tokens
```

#### 3. Responsive Component
```
Crea estilos responsive para card grid:
- Mobile: 1 columna
- Tablet: 2 columnas
- Desktop: 3 columnas
Usa CSS Grid y media queries
```

---

## ✅ Checklist de Estilos Consistentes

### Design Tokens
- [ ] Variables CSS para colores, tipografía, spacing
- [ ] Paleta de colores con shades (50-900)
- [ ] Typography scale definido
- [ ] Spacing basado en grid (4px o 8px)
- [ ] Border radius tokens
- [ ] Shadow tokens

### Naming Conventions
- [ ] Metodología elegida (BEM, SMACSS, utility-first)
- [ ] Nombres descriptivos y consistentes
- [ ] Evitar anidamiento profundo (> 3 niveles)
- [ ] Prefijos para componentes reutilizables

### Componentes
- [ ] Sistema de botones (variants, sizes, states)
- [ ] Sistema de forms (inputs, labels, errors)
- [ ] Cards reutilizables
- [ ] Modal/Dialog patterns
- [ ] Navigation components

### Responsive
- [ ] Mobile-first approach
- [ ] Breakpoints consistentes
- [ ] Grid system definido
- [ ] Touch targets ≥ 44px en mobile

### Performance
- [ ] CSS minificado en producción
- [ ] Critical CSS inline
- [ ] Lazy load de CSS no crítico
- [ ] Purge de CSS no usado (PurgeCSS, Tailwind)

---
