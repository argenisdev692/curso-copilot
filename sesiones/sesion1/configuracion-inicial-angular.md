# 🔧 Configuración Inicial de Copilot en Proyectos Angular

## 📋 Prompts para Setup Básico de Angular con GitHub Copilot

### 🎯 Prompt Principal para Configuración Completa

**Copia este prompt completo en Copilot Chat:**

"Configura un nuevo proyecto Angular 19 desde cero con las mejores prácticas para usar GitHub Copilot. Incluye:

1. **Instalación y setup inicial:**
   - Crear proyecto con Angular CLI
   - Instalar dependencias esenciales
   - Configurar TypeScript strict mode
   - Setup de ESLint y Prettier

2. **Configuración de Copilot:**
   - Archivo .cursorrules o .instructions.md optimizado para Angular
   - Configuración de VS Code para desarrollo Angular
   - Extensiones recomendadas

3. **Estructura del proyecto:**
   - Carpetas por feature
   - Componentes standalone
   - Servicios inyectables
   - Configuración de rutas con lazy loading

4. **Configuración de desarrollo:**
   - Scripts de package.json
   - Configuración de Angular DevKit
   - Setup de testing con Jasmine/Karma
   - Configuración de build optimization

Genera comandos paso a paso, archivos de configuración, y explica cada decisión para un proyecto Angular profesional con Copilot."

---

## 🛠️ Prompts Específicos por Categoría

### 1. Creación del Proyecto Angular

**Prompt:** "Crea un nuevo proyecto Angular 19 con las siguientes características optimizadas para Copilot: standalone components, strict TypeScript, ESLint, y configuración moderna. Incluye comandos CLI y explica cada flag usado."

**Resultado esperado:**
```bash
ng new mi-proyecto-angular --standalone --strict --package-manager=npm --routing --style=scss
```

### 2. Configuración de TypeScript para Copilot

**Prompt:** "Genera un tsconfig.json optimizado para Angular 19 con strict mode completo, paths para imports absolutos, y configuraciones que maximicen las sugerencias de Copilot. Explica cada opción."

**Archivo generado:** `tsconfig.json` con configuraciones avanzadas.

### 3. Setup de ESLint y Prettier

**Prompt:** "Configura ESLint y Prettier para un proyecto Angular con reglas que complementen GitHub Copilot. Incluye configuración para TypeScript, Angular, y mejores prácticas de código."

**Archivos generados:**
- `.eslintrc.json`
- `.prettierrc`
- `.prettierignore`

### 4. Configuración de VS Code para Angular

**Prompt:** "Genera settings.json para VS Code optimizado para desarrollo Angular con Copilot. Incluye extensiones recomendadas, formateo automático, y atajos para productividad."

**Archivo generado:** `.vscode/settings.json`

### 5. Archivo de Instrucciones para Copilot

**Prompt:** "Crea un archivo .cursorrules o .instructions.md con instrucciones específicas para GitHub Copilot en proyectos Angular. Incluye reglas para standalone components, signals, control flow, y patrones de diseño."

**Archivo generado:** `.cursorrules` con instrucciones detalladas.

### 6. Estructura de Carpetas por Feature

**Prompt:** "Genera la estructura de carpetas recomendada para una aplicación Angular enterprise con separación por features. Incluye convenciones de nomenclatura y organización lógica."

**Estructura sugerida:**
```
src/
├── app/
│   ├── features/
│   │   ├── auth/
│   │   ├── dashboard/
│   │   └── users/
│   ├── shared/
│   │   ├── components/
│   │   ├── services/
│   │   └── models/
│   └── core/
```

### 7. Configuración de Testing

**Prompt:** "Configura Jasmine y Karma para testing en Angular con configuraciones optimizadas. Incluye setup para tests unitarios, integración, y utilidades para testing con Copilot."

**Archivos generados:**
- `karma.conf.js`
- `test.ts`
- Configuraciones de testing

### 8. Scripts de Package.json Optimizados

**Prompt:** "Genera scripts npm optimizados para desarrollo Angular con Copilot. Incluye comandos para build, test, lint, format, y desarrollo con hot reload."

**Scripts sugeridos:**
```json
{
  "scripts": {
    "start": "ng serve --open",
    "build": "ng build --configuration production",
    "test": "ng test",
    "lint": "ng lint",
    "format": "prettier --write .",
    "prepare": "husky install"
  }
}
```

### 9. Configuración de Git Hooks

**Prompt:** "Configura Husky y lint-staged para automatizar linting y formateo antes de commits. Optimiza el workflow de desarrollo con Copilot."

**Archivos generados:**
- `.husky/pre-commit`
- `package.json` con lint-staged

### 10. Setup de Angular Material (Opcional)

**Prompt:** "Instala y configura Angular Material con tema personalizado optimizado para Copilot. Incluye componentes esenciales y configuración de theming."

---

## 🚀 Checklist de Configuración Completa

- [ ] Proyecto Angular creado con CLI
- [ ] TypeScript configurado en strict mode
- [ ] ESLint y Prettier configurados
- [ ] VS Code settings optimizados
- [ ] Archivo de instrucciones para Copilot creado
- [ ] Estructura de carpetas implementada
- [ ] Testing configurado
- [ ] Scripts npm optimizados
- [ ] Git hooks configurados
- [ ] Dependencias de desarrollo instaladas

## 💡 Consejos para Usar Copilot en la Configuración

1. **Usa prompts descriptivos** - Copilot genera mejor código cuando das contexto claro
2. **Itera sobre las sugerencias** - Si no te gusta la primera, pide modificaciones
3. **Verifica las configuraciones** - Siempre revisa que las configs generadas sean correctas
4. **Adapta a tus necesidades** - Modifica las sugerencias según tu stack tecnológico
5. **Documenta tus decisiones** - Mantén un README con las elecciones de configuración

---

**Nota:** Esta guía está optimizada para Angular 19 y las mejores prácticas de desarrollo con GitHub Copilot en 2025.