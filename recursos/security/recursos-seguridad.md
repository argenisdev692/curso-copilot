# Recursos de Seguridad y Mejores Prácticas

## 📚 GUÍAS DE SEGURIDAD Y MEJORES PRÁCTICAS

### OWASP (Open Web Application Security Project)
- **OWASP Top 10 (2021)**: https://owasp.org/www-project-top-ten/
  - Lista actualizada de las 10 vulnerabilidades web más críticas
  - Descarga gratuita en PDF
  - **Nota 2025**: OWASP Top 10:2025 se anunciará en noviembre 2025 en OWASP Global AppSec USA, con enfoque en amenazas emergentes como IA, supply chain y APIs modernas.

- **OWASP Top 10 for LLM Applications (2025)**: https://genai.owasp.org/llm-top-10/
  - Riesgos específicos para aplicaciones con modelos de lenguaje grande (LLM) como GitHub Copilot
  - Incluye amenazas como prompt injection, data poisoning y excessive agency
  - Parte del OWASP GenAI Security Project

- **OWASP Cheat Sheet Series**: https://cheatsheetseries.owasp.org/
  - Guías prácticas para implementar seguridad
  - Cheat sheets para APIs, autenticación, criptografía, y ahora IA/LLM

- **OWASP Testing Guide v4.2 (2020)**: https://owasp.org/www-project-web-security-testing-guide/
  - Metodología completa para testing de seguridad
  - **Nota 2025**: Versión 5.0 en desarrollo, con actualizaciones para amenazas modernas

- **OWASP GenAI Security Project**: https://genai.owasp.org/
  - Recursos completos para seguridad en IA generativa y LLM
  - Incluye Threat Defense COMPASS, Agentic Security Initiative y más

### Microsoft Security
- **Security Development Lifecycle (SDL)**: https://www.microsoft.com/en-us/securityengineering/sdl/
  - Framework de Microsoft para desarrollo seguro
  - Herramientas y templates gratuitos

- **Azure Security Center**: https://azure.microsoft.com/en-us/services/security-center/
  - Guías de mejores prácticas para Azure

### .NET Security
- **Secure Coding Guidelines**: https://docs.microsoft.com/en-us/dotnet/standard/security/secure-coding-guidelines
  - Guías oficiales de Microsoft para .NET

- **OWASP .NET Security Cheat Sheet**: https://cheatsheetseries.owasp.org/cheatsheets/DotNet_Security_Cheat_Sheet.html

### Recursos en Español
- **INCIBE (Instituto Nacional de Ciberseguridad)**: https://www.incibe.es/
  - Guías de ciberseguridad en español
  - Recursos gratuitos para empresas

- **Agencia Española de Protección de Datos (AEPD)**: https://www.aepd.es/
  - Guías GDPR y LOPD en español

### Herramientas de Análisis
- **SonarQube**: https://www.sonarsource.com/products/sonarqube/
  - Análisis estático de código con reglas de seguridad
  - Soporte para .NET, JavaScript/TypeScript y más

- **Snyk**: https://snyk.io/
  - Escaneo de vulnerabilidades en dependencias
  - Integración con GitHub Copilot para sugerencias de seguridad

- **OWASP ZAP**: https://www.zaproxy.org/
  - Proxy de interceptación para testing de seguridad
  - **Nota 2025**: Integración mejorada con herramientas de IA para testing automatizado

- **CodeQL (GitHub)**: https://codeql.github.com/
  - Análisis semántico avanzado para detectar vulnerabilidades
  - Integrado con GitHub Copilot para análisis en tiempo real

- **Semgrep**: https://semgrep.dev/
  - Análisis estático con reglas personalizables
  - Soporte para lenguajes modernos y patrones de IA

### Cursos y Certificaciones
- **OWASP Web Application Security**: https://owasp.org/www-pdf-archive/OWASP_Web_Application_Security_Testing_Checklist_v1_0.pdf
- **Microsoft Security Development Lifecycle**: https://www.microsoft.com/en-us/securityengineering/sdl/
- **Certified Secure Software Lifecycle Professional (CSSLP)**: https://www.isc2.org/certifications/csslp

### Comunidades y Foros
- **OWASP Community**: https://owasp.org/community/
- **Reddit r/netsec**: https://www.reddit.com/r/netsec/
- **Stack Overflow Security**: https://stackoverflow.com/questions/tagged/security

---

## 🔧 HERRAMIENTAS DE SEGURIDAD PARA DESARROLLADORES

### Análisis Estático de Código
- **SonarQube**: Análisis continuo de calidad y seguridad
- **CodeQL**: Motor de análisis semántico de GitHub
- **Semgrep**: Análisis estático con reglas personalizables

### Escaneo de Dependencias
- **Snyk**: Monitoreo continuo de vulnerabilidades
- **OWASP Dependency Check**: Escaneo local de dependencias
- **npm audit**: Herramienta integrada de Node.js

### Testing de Seguridad
- **OWASP ZAP**: Proxy de interceptación automatizado
- **Burp Suite Community**: Suite completa de testing web
- **Postman Security**: Testing de APIs con enfoque en seguridad

### Gestión de Secretos
- **Azure Key Vault**: Gestión centralizada de secretos
- **AWS Secrets Manager**: Servicio de gestión de secretos
- **HashiCorp Vault**: Gestión de secretos y cifrado

---

## 📋 CHECKLISTS DE SEGURIDAD

### Checklist de Desarrollo Seguro
- [ ] Validación de entrada en todas las APIs
- [ ] Uso de parámetros en consultas SQL
- [ ] Manejo seguro de autenticación y autorización
- [ ] Cifrado de datos sensibles en tránsito y reposo
- [ ] Configuración correcta de CORS
- [ ] Validación de tokens JWT
- [ ] Rate limiting implementado
- [ ] Logs sin información sensible

### Checklist para Uso de IA en Desarrollo (GitHub Copilot 2025)
- [ ] Revisar código generado por IA antes de aceptar
- [ ] Verificar que no contiene credenciales hardcodeadas
- [ ] Configurar exclusiones de archivos sensibles en GitHub Copilot
- [ ] Usar prompts específicos para código seguro
- [ ] Documentar origen del código (IA vs humano)
- [ ] Auditar código generado antes de producción
- [ ] Mantener plan B si la IA falla
- [ ] **Nuevo 2025**: Usar Agent Mode para tareas complejas con verificación
- [ ] **Nuevo 2025**: Revisar integraciones MCP para seguridad
- [ ] **Nuevo 2025**: Validar contra OWASP Top 10 for LLM Applications

### Checklist de Despliegue Seguro
- [ ] Variables de entorno configuradas correctamente
- [ ] Secrets no expuestos en repositorios
- [ ] Configuración de firewall adecuada
- [ ] Certificados SSL válidos
- [ ] Headers de seguridad configurados
- [ ] Monitoreo y logging habilitados

---

## 🎯 PROMPTS PARA GITHUB COPILOT - CÓDIGO SEGURO (2025)

### Generar Código Seguro con GitHub Copilot

**Prompt para API Controller Seguro:**
```
Crea un controlador ASP.NET Core API seguro que incluya:
- Validación de entrada con Data Annotations
- Manejo de errores sin exponer información interna
- Logging seguro sin datos sensibles
- Rate limiting básico
- CORS configurado correctamente
- Autenticación y autorización
- Protección contra OWASP Top 10 vulnerabilidades
```

**Prompt para Servicio de Base de Datos Seguro:**
```
Crea un servicio de base de datos que incluya:
- Uso de IConfiguration para connection strings
- Parámetros en todas las consultas SQL
- Manejo de transacciones
- Logging de operaciones sin datos sensibles
- Validación de permisos de acceso
- Protección contra inyección SQL
```

**Prompt para Autenticación Segura:**
```
Implementa autenticación JWT segura que incluya:
- Hash de contraseñas con BCrypt
- Validación de tokens
- Refresh tokens
- Manejo de expiración
- Protección contra ataques comunes
- Cumplimiento con OWASP Top 10 for LLM Applications
```

**Prompt para Integración con IA Segura (2025):**
```
Crea una integración con GitHub Copilot que incluya:
- Validación de prompts para evitar injection
- Sanitización de outputs generados por IA
- Logging de interacciones con IA sin datos sensibles
- Rate limiting para llamadas a modelos de IA
- Manejo de errores cuando la IA falla
- Verificación de integridad de respuestas de IA
```

---

## 📖 LECTURAS RECOMENDADAS

### Libros
- **"The Web Application Hacker's Handbook"** - Dafydd Stuttard
- **"Hacking: The Art of Exploitation"** - Jon Erickson
- **"Secure Coding in C and C++"** - Robert Seacord

### Blogs y Newsletters
- **OWASP Blog**: https://owasp.org/blog/
- **OWASP GenAI Security Project Blog**: https://genai.owasp.org/blog/
- **Microsoft Security Blog**: https://www.microsoft.com/en-us/security/blog/
- **Krebs on Security**: https://krebsonsecurity.com/
- **GitHub Security Lab Blog**: https://github.blog/category/security/

### Podcasts
- **Security Now**: Podcast técnico sobre seguridad
- **Darknet Diaries**: Historias reales de ciberseguridad
- **The CyberWire**: Noticias diarias de ciberseguridad

---

## 🏢 RECURSOS PARA EMPRESAS

### Frameworks de Cumplimiento
- **NIST Cybersecurity Framework**: https://www.nist.gov/cyberframework
- **ISO 27001**: Estándar internacional de gestión de seguridad
- **PCI DSS**: Para procesamiento de tarjetas de crédito

### Herramientas Empresariales
- **Microsoft Defender for Cloud**: Seguridad integral para Azure
- **AWS Security Hub**: Consola centralizada de seguridad
- **Google Cloud Security Command Center**: Gestión de seguridad unificada
- **GitHub Advanced Security**: CodeQL, secret scanning y dependency review
- **OWASP Threat Defense COMPASS**: Framework para evaluación de amenazas en IA

### Servicios de Consultoría
- **OWASP Services**: https://owasp.org/supporters/
- **Microsoft Security Services**: Servicios profesionales de seguridad
- **Certified Security Consultancies**: Firmas certificadas en ciberseguridad

---

## 🆕 RECURSOS ESPECÍFICOS PARA IA Y GITHUB COPILOT (2025)

### Seguridad en IA Generativa
- **OWASP Top 10 for LLM Applications**: https://genai.owasp.org/llm-top-10/
  - Riesgos específicos para aplicaciones con IA
  - Incluye prompt injection, data poisoning, excessive agency

- **GitHub Copilot Security Guide**: https://docs.github.com/en/copilot/security
  - Mejores prácticas para uso seguro de Copilot
  - Configuración de filtros y exclusiones

### Herramientas de Seguridad para IA
- **GitHub Advanced Security**: Integrado con Copilot para análisis en tiempo real
- **OWASP Threat Defense COMPASS**: Evaluación de amenazas en aplicaciones con IA
- **Agentic Security Initiative**: Recursos para agentes autónomos seguros

### Certificaciones y Entrenamiento 2025
- **OWASP Certified Secure Software Developer (OCSD)**: Nueva certificación enfocada en desarrollo seguro con IA
- **GitHub Security Certification**: Para desarrolladores que usan Copilot
- **Microsoft Certified: Azure AI Engineer Associate**: Incluye seguridad en IA

---

*Última actualización: Noviembre 2025*
*Recursos sujetos a cambios en URLs y disponibilidad*