# Asistente de Optimización de Rendimiento en Angular 19

## 🎯 Propósito
Optimizar el rendimiento y la carga de módulos en una aplicación Angular 19 frontend, implementando mejores prácticas para mejorar tiempos de carga, detección de cambios, y experiencia de usuario.

## 🛣️ Routing & Preloading
- app.config.ts: Implementar PreloadAllModules con QuicklinkStrategy
- Custom preloading: Priorizar dashboard/tickets sobre users
- Network-aware: Solo precargar en WiFi/4G rápido

## 🔄 Change Detection
- Todos los componentes: ChangeDetectionStrategy.OnPush
- login.component.ts, register.component.ts, dashboard.component.ts
- Usar signals para evitar CD innecesario
- trackBy functions en listas de tickets/users/comments

## 🏗️ Build Optimization
- angular.json production config:
  - optimization: true con inlineCriticalCss
  - buildOptimizer: true
  - namedChunks: false (producción)
  - budgets: Reducir a 300kB initial
  - sourceMap: false en prod

## 📜 Virtual Scrolling
- Instalar @angular/cdk
- ticket-list.component: Implementar cdk-virtual-scroll-viewport
- Altura fija items: 80px por ticket
- Buffer: 5 items arriba/abajo

## 📱 PWA & Caching
- ng add @angular/pwa
- ngsw-config.json: Cache API responses, assets, fonts
- Offline-first para dashboard
- Update prompts para nueva versión

## 🖼️ Image Optimization
- Reemplazar <img> por <img ngSrc> en todos los componentes
- Lazy loading nativo de imágenes
- Responsive breakpoints

## 🎯 Resultado Esperado
- Initial bundle < 300kB
- LCP < 2.5s
- TTI < 3.5s
- Lighthouse score > 90
- Soporte offline básico