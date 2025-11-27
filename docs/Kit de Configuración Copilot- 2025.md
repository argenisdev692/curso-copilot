# 🚀 Kit de Configuración Inicial

## Curso: GitHub Copilot para Desarrolladores Web (.Net y Angular)

> **Fechas:** 11 Nov - 4 Dic 2025 | **Grupo:** 1 | **Tecnologías:** .NET + Angular

---

## 📋 Requisitos Previos

### Hardware Mínimo

- **CPU:** Intel i5 o superior
- **RAM:** 8 GB (16 GB recomendado)
- **Disco:** 50 GB libres
- **GPU (opcional):** NVIDIA con 4 GB VRAM

### Sistema Operativo

- Windows 11 / macOS 12+ / Ubuntu 22.04+

---

## 🛠️ Instalaciones Base

### 1. Visual Studio Code

Descargar e instalar desde: https://code.visualstudio.com/

### 2. Git

Descargar desde: https://git-scm.com/

**Configuración inicial:**

```bash
git config --global user.name "Tu Nombre"
git config --global user.email "tu@email.com"
```

### 3. Docker Desktop

Descargar desde: https://www.docker.com/products/docker-desktop

### 4. .NET SDK 8.0

Descargar desde: https://dotnet.microsoft.com/download

Verificar con: `dotnet --version`

### 5. Node.js LTS (v20.x)

Descargar desde: https://nodejs.org/

Verificar con: `node --version` y `npm --version`

---

## 🧩 Extensiones de VSCode

### 📥 Importación Automática

En la carpeta `recursos/vscode-extensions/` encontrarás el archivo:

- **`extensions.txt`** - Lista completa para importar

**Instrucciones de instalación:**

1. **Opción A - Importación automática (recomendado):**

   - Abre VSCode
   - Ve a la carpeta `recursos/vscode-extensions/`
   - Ejecuta el script correspondiente:
     - Windows: `install-extensions.bat`
     - Mac/Linux: `bash install-extensions.sh`

2. **Opción B - Manual:**
   - Abre VSCode
   - Presiona `Ctrl+Shift+X` (Windows/Linux) o `Cmd+Shift+X` (Mac)
   - Busca e instala cada extensión de la lista

### 📝 Lista de Extensiones Esenciales

#### 🔥 Imprescindibles

- **GitHub Copilot** - Asistente IA principal (oficial GitHub)
- **GitHub Copilot Chat** - Asistente conversacional avanzado
- **ESLint** - Linting JavaScript/TypeScript
- **Prettier** - Formateo de código

#### ⚙️ Backend (.NET/C#)

- **C# Dev Kit** - Desarrollo .NET completo (oficial Microsoft)
- **C#** - Soporte básico C# (oficial Microsoft)
- **C# Extensions** - Snippets y herramientas C#
- **.NET Core Test Explorer** - Ejecutar tests desde VSCode
- **NuGet Package Manager** - Gestión de paquetes NuGet
- **Code Runner** - Ejecutar código rápidamente

#### ⚛️ Frontend (Angular/TypeScript)

- **Angular Language Service** - IntelliSense para templates Angular
- **TypeScript and JavaScript Language Features** - Soporte avanzado TypeScript
- **Angular Snippets** - Snippets para Angular (johnpapa.Angular2)
- **Auto Rename Tag** - Renombrar etiquetas automáticamente
- **Auto Close Tag** - Cerrar etiquetas automáticamente
- **Tailwind CSS IntelliSense** - Autocompletado Tailwind
- **npm Intellisense** - Autocompletado imports npm

#### 🗄️ Base de Datos

- **MongoDB for VS Code** - Cliente MongoDB integrado

#### 🐳 DevOps y Git

- **Docker** - Gestión de contenedores
- **GitLens** - Git avanzado
- **Git Graph** - Visualización de commits
- **GitHub Actions** - Soporte para workflows CI/CD
- **REST Client** - Pruebas de API

#### 🎨 UI y Productividad

- **Material Icon Theme** - Iconos visuales
- **One Dark Pro** - Tema recomendado
- **Path Intellisense** - Autocompletado rutas
- **EditorConfig** - Consistencia de formato
- **Live Share** - Colaboración en tiempo real
- **Better Comments** - Comentarios categorizados para prompt engineering
- **Import Cost** - Ver tamaño de paquetes importados

---

## 🤖 Configuración de GitHub Copilot

### Paso 1: Crear Cuenta GitHub

1. Ve a: https://github.com/
2. Regístrate con cuenta gratuita o usa una existente
3. **Nota 2025**: Copilot requiere cuenta GitHub verificada

### Paso 2: Activar GitHub Copilot

1. Ve a: https://github.com/settings/copilot
2. Activa GitHub Copilot (versión gratuita o paga)
3. **Nota 2025**: Incluye nuevas funcionalidades como Agent Mode y MCP

### Paso 3: Instalar Extensión en VSCode

1. Abre VSCode
2. Presiona `Ctrl+Shift+X` / `Cmd+Shift+X`
3. Busca: **"GitHub Copilot"**
4. Instala la extensión oficial
5. Inicia sesión con tu cuenta GitHub

### Paso 4: Verificar

- Crea un archivo `.cs` o `.tsx`
- Comienza a escribir
- Deberías ver sugerencias con el logo de GitHub Copilot
- Presiona **Tab** para aceptar sugerencias
- **Nota 2025**: Prueba Agent Mode con `Ctrl+Enter` para conversaciones avanzadas

---

## 🏃 MongoDB - Configuración Rápida

### Opción 1: MongoDB con Docker (Recomendado)

Navega a la carpeta `recursos/docker/` y ejecuta:

```bash
docker-compose up -d
```

Esto levantará:

- **MongoDB** en `localhost:27017`
- **Mongo Express** (UI) en `http://localhost:8081`

**Credenciales por defecto:**

- Usuario: `admin`
- Contraseña: `password123`

### Opción 2: MongoDB Atlas (Cloud)

1. Crea cuenta gratuita en: https://www.mongodb.com/cloud/atlas
2. Crea un cluster
3. Obtén tu connection string
4. Úsalo en la extensión MongoDB de VSCode

### Conectar desde VSCode

1. Abre extensión MongoDB (icono lateral)
2. Click **"Add Connection"**
3. Pega: `mongodb://admin:password123@localhost:27017`

---

## 📸 Capturas de Referencia

En la carpeta `docs/copilot_guide/screenshots/` encontrarás imágenes detalladas del proceso completo de instalación y configuración de GitHub Copilot:

- ✅ GitHub Copilot correctamente configurado
- ✅ Extensiones instaladas
- ✅ MongoDB conectado
- ✅ Docker funcionando
- ✅ Configuración settings.json de ejemplo

**Nota:** Las capturas ahora están organizadas en `docs/copilot_guide/` junto con las guías completas de instalación (`01_installation_steps.md`) y uso avanzado (`02_usage_and_features.md`).

---

## 🔧 Configuración del Repositorio del Curso

### Clonar el Repositorio

**Usando el repositorio del curso:**

```bash
# Clonar el repositorio del curso
git clone https://bitbucket.org/virtual-sessions/curso-copilot.git

# Entrar al directorio
cd curso-copilot

# Configurar tu nombre (para futuras referencias)
git config user.name "Tu Nombre"
git config user.email "tu@email.com"
```

**Importante:**

- El repositorio permite **lectura** del contenido del curso
- Si necesitas hacer cambios, contacta al instructor

**Verificar que el repositorio se clonó correctamente:**

```bash
# Ver archivos del repositorio
ls -la

# Ver estado de Git
git status

# Ver rama actual
git branch
```

---

## 💻 Instalación y Verificación de .NET (C#)

### 1. Descargar .NET SDK

- Ve a: https://dotnet.microsoft.com/download
- Descarga **.NET 8.0 SDK** (última versión)
- Ejecuta el instalador
- **Importante:** Marca "Add to PATH" durante instalación

### 2. Verificar Instalación

```bash
dotnet --version
```

Deberías ver: `8.0.100` o superior

**Si `dotnet --version` da error** (aunque hayas marcado "Add to PATH"):

#### Agregar .NET al PATH (si ya está instalado)

**Opción A: Mediante Interfaz Gráfica**

1. Presiona `Win + R` y escribe: `sysdm.cpl` → Enter
2. Ve a la pestaña "Opciones avanzadas"
3. Click en "Variables de entorno"
4. En "Variables del sistema", busca `Path` y haz doble click
5. Click en "Nuevo" y agrega:
   ```
   C:\Program Files\dotnet
   ```
6. Click OK en todas las ventanas

**Opción B: Mediante PowerShell (Administrador)**

```powershell
# Ejecuta PowerShell como Administrador
[Environment]::SetEnvironmentVariable("Path", $env:Path + ";C:\Program Files\dotnet", "Machine")
```

**Paso 2.1: Reiniciar Terminal/VSCode**
⚠️ **MUY IMPORTANTE:**

- Cierra TODAS las ventanas de VSCode
- Cierra TODAS las terminales (CMD, PowerShell)
- Abre una nueva terminal o nuevo VSCode
- Verifica: `dotnet --version`

### 3. Crear "Hola Mundo" API

```bash
# Crear proyecto API Web
dotnet new webapi -n HolaMundoAPI

# Entrar al directorio
cd HolaMundoAPI

# Ejecutar la API
dotnet run
```

### 4. Probar la API

**La aplicación mostrará URLs como:**

```
Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
```

**Si el puerto no se muestra o ves el warning `Failed to determine the https port for redirect`**:

El warning aparece porque `app.UseHttpsRedirection()` en `Program.cs` intenta redirigir a HTTPS sin un puerto configurado. Para evitarlo:

**Opción A: Comentar la redirección HTTPS (si solo usarás HTTP)**
Edita `Program.cs` y comenta la línea:

```csharp
// app.UseHttpsRedirection();
```

**Opción B: Ejecutar con el perfil HTTPS configurado**
Ejecuta con el perfil HTTPS desde VSCode o usando:

```bash
dotnet run --launch-profile https
```

**Opción C: Configurar URLs explícitamente**

```bash
dotnet run --urls="http://localhost:5000"
```

Así tu API funcionará correctamente en `http://localhost:<puerto>` sin mostrar advertencias.

**Opciones para probar:**

- Abre navegador en: **http://localhost:5000/swagger** (recomendado para empezar)
- O prueba directamente: **http://localhost:5000/weatherforecast**

**Nota sobre HTTPS:** Si aparece advertencia de certificado, es normal en desarrollo. Puedes:

- Usar HTTP (puerto 5000) para evitar problemas
- O ejecutar: `dotnet dev-certs https --trust` para confiar en el certificado

### Comandos Útiles .NET

```bash
dotnet new console -n MiApp       # Crear consola
dotnet new webapi -n MiAPI       # Crear API
dotnet build                     # Compilar
dotnet run                       # Ejecutar
dotnet add package NombrePaquete # Agregar NuGet
dotnet watch run                 # Ejecutar con hot reload
```

---

## ⚛️ Instalación y Verificación de Angular

### 1. Verificar Node.js

```bash
node --version    # Debe ser v20.x o superior
npm --version     # Debe ser v10.x o superior
```

### 2. Crear "Hola Mundo" Angular con CLI

```bash
# Instalar Angular CLI globalmente
npm install -g @angular/cli

# Crear proyecto Angular con CLI
ng new hola-mundo-angular --routing --style=css --skip-git

# Entrar al directorio
cd hola-mundo-angular

# Instalar dependencias
npm install

# Ejecutar en modo desarrollo
npm start
```

### 3. Verificar Funcionamiento

La terminal mostrará:

```
Local:   http://localhost:4200/
```

- Abre tu navegador en: **http://localhost:4200**
- Deberías ver la página inicial de Angular

### 4. Crear Componente de Prueba

Edita `src/app/app.component.html`:

```html
<div class="container">
  <h1>¡Hola Mundo con GitHub Copilot!</h1>
  <p>Curso: GitHub Copilot para Desarrolladores Web</p>
  <p>Sesión 1 - Introducción a GitHub Copilot</p>
  <p>Powered by Angular 🅰️</p>
</div>
```

Guarda y verifica que se actualiza **instantáneamente** en el navegador (Hot Module Replacement).

### Comandos Útiles Angular

```bash
ng serve               # Iniciar desarrollo (puerto 4200)
ng build              # Compilar para producción
ng generate component nombre-componente  # Crear componente
npm install nombre-paquete # Instalar paquete
```

---

## ✅ Checklist Final

Antes del primer día del curso, verifica:

**Herramientas Base:**

- [ ] Visual Studio Code instalado y funcionando
- [ ] Git configurado con tu usuario
- [ ] Repositorio del curso clonado desde Bitbucket
- [ ] Docker Desktop corriendo

**Desarrollo .NET:**

- [ ] .NET SDK 8.0 instalado (`dotnet --version` muestra 8.0.x)
- [ ] API "Hola Mundo" .NET funcionando en http://localhost:5000
- [ ] Swagger UI accesible en http://localhost:5000/swagger

**Desarrollo Angular:**

- [ ] Node.js v20.x y npm instalados
- [ ] Angular CLI instalado globalmente
- [ ] App "Hola Mundo" Angular funcionando en http://localhost:4200
- [ ] Hot reload funciona al editar archivos

**Desarrollo Angular:**

- [ ] Node.js v20.x y npm instalados
- [ ] Angular CLI instalado globalmente
- [ ] App "Hola Mundo" Angular funcionando en http://localhost:4200
- [ ] Hot reload funciona al editar archivos

**Extensiones y Herramientas:**

- [ ] Todas las extensiones de VSCode instaladas
- [ ] GitHub Copilot activado y con sesión iniciada (ver logo en sugerencias)
- [ ] MongoDB corriendo (local o cloud)
- [ ] MongoDB conectado desde extensión VSCode

---

## 🆘 Troubleshooting

### Git: "'git' is not recognized"

- Reinicia terminal/PowerShell
- Verifica instalación: `git --version`
- Reinstala Git y asegúrate de marcar "Add to PATH"

### .NET: "'dotnet' is not recognized"

- Reinicia PowerShell/CMD completamente
- Verifica que .NET esté en PATH
- Reinstala .NET SDK marcando "Add to PATH"

### .NET: Error de certificado HTTPS

```bash
# Confiar en certificado de desarrollo
dotnet dev-certs https --trust

# O usa HTTP en su lugar
dotnet run --urls="http://localhost:5000"
```

### Angular: "ng: command not found"

- Instala Angular CLI: `npm install -g @angular/cli`
- Reinicia terminal
- Verifica instalación: `ng version`

### Angular: Error al crear proyecto

- Verifica Node.js: `node --version` (debe ser v20+)
- Verifica Angular CLI: `ng version`
- Limpia caché npm: `npm cache clean --force`
- Intenta de nuevo con: `ng new`

### Puerto ocupado (5000, 5173, 27017)

**Para .NET (puerto 5000/5001):**

```bash
dotnet run --urls="http://localhost:5002"
```

**Para Angular (puerto 4200):**

- Angular automáticamente usará el siguiente puerto disponible
- O edita `angular.json` para especificar puerto

**Para MongoDB (puerto 27017):**

- Detén otros servicios MongoDB: `docker stop <container-id>`
- O cambia puerto en `docker-compose.yml`

### GitHub Copilot no aparece

- Reinicia VSCode completamente (cerrar todas las ventanas)
- Verifica que iniciaste sesión: `Ctrl+Shift+P` → "GitHub Copilot: Sign in"
- Revisa que tienes GitHub Copilot activado en github.com/settings/copilot
- Verifica en esquina inferior derecha si hay icono de GitHub Copilot
- **Nota 2025**: Asegúrate de tener cuenta GitHub verificada

### Docker no inicia

- Verifica que Docker Desktop está corriendo
- **Windows:** Verifica que WSL2 está habilitado
- Reinicia el servicio Docker desde Docker Desktop
- Revisa logs en Docker Desktop

### MongoDB no conecta

- Verifica que el contenedor está corriendo: `docker ps`
- Deberías ver contenedor con nombre que incluye "mongo"
- Prueba acceso web: http://localhost:8081
- Revisa credenciales: `admin` / `password123`
- Verifica que puerto 27017 no está ocupado

### Extensiones no se instalan

- Verifica conexión a internet
- Reinicia VSCode completamente
- Intenta instalar manualmente desde el marketplace
- Revisa logs: `Ctrl+Shift+P` → "Developer: Show Logs"

### Visual Studio Code: Rendimiento lento

- Deshabilita extensiones no necesarias temporalmente
- Aumenta memoria si tienes <8GB RAM
- Cierra proyectos grandes cuando no los uses

---

## 📞 Soporte Pre-Curso

Si tienes problemas con la configuración antes del inicio del curso:

- Revisa las capturas en `docs/tabnine_guide/screenshots/`
- Consulta el archivo `TROUBLESHOOTING.md` en la carpeta recursos
- Contacta al equipo de soporte del curso

---

## 📂 Estructura de Carpetas del Curso

```
copilot-curso-2025/
├── docs/                            # Documentación completa del curso
│   ├── Kit de Configuración Copilot- 2025.md
│   ├── Kit de Configuración Copilot- 2025.pdf
│   └── copilot_guide/               # ✨ Guía completa de GitHub Copilot
│       ├── README.md                # Índice y resumen de la guía
│       ├── 01_installation_steps.md # Guía detallada de instalación
│       ├── 02_usage_and_features.md # Consejos de uso y funcionalidades
│       └── screenshots/             # Capturas de pantalla numeradas
│           ├── 01-download-extension.png
│           ├── 02-vscode-installation.png
│           ├── 03-first-activation.png
│           ├── 04-account-creation.png
│           ├── 05-account-types.png
│           ├── 06-pro-activation.png
│           ├── 07-initial-configuration.png
│           ├── 08-copilot-panel.png
│           ├── 09-functionality-check.png
│           └── 10-first-use.png
├── recursos/                        # Recursos del curso
│   ├── configuracion/               # Configuraciones del curso
│   │   └── copilot/                 # Configuración específica de Copilot
│   │       └── config-recomendada.json
│   ├── docker/                      # Docker compose para desarrollo
│   └── vscode-extensions/           # Scripts y perfiles para instalar extensiones
├── sesion1/                         # 📂 SESIÓN 1 - 11 noviembre
├── sesion2/                         # 📂 SESIÓN 2 - 13 noviembre
├── sesion3/                         # 📂 SESIÓN 3 - 18 noviembre
├── sesion4/                         # 📂 SESIÓN 4 - 20 noviembre
├── sesion5/                         # 📂 SESIÓN 5 - 25 noviembre
├── sesion6/                         # 📂 SESIÓN 6 - 27 noviembre
├── sesion7/                         # 📂 SESIÓN 7 - 02 diciembre
├── sesion8/                         # 📂 SESIÓN 8 - 04 diciembre
├── README.md                        # Documentación principal
└── .gitignore                       # Git ignore recomendado
```

---

## 📊 Resumen de URLs y Puertos

| Servicio         | URL                           | Puerto | Notas                         |
| ---------------- | ----------------------------- | ------ | ----------------------------- |
| API .NET (HTTP)  | http://localhost:5000         | 5000   | Recomendado para inicio       |
| API .NET (HTTPS) | https://localhost:5001        | 5001   | Requiere certificado confiado |
| Swagger .NET     | http://localhost:5000/swagger | 5000   | Documentación API             |
| Angular          | http://localhost:4200         | 4200   | Puerto por defecto Angular    |
| MongoDB          | mongodb://localhost:27017     | 27017  | Base de datos                 |
| Mongo Express    | http://localhost:8081         | 8081   | UI web MongoDB                |

---

## 🎓 Próximos Pasos

Una vez completada esta configuración:

1. ✅ Verifica el checklist completo
2. 📸 Toma capturas si todo funciona
3. 🧪 Experimenta creando archivos y usando GitHub Copilot
4. 📚 Revisa material de la Sesión 1 en el repositorio
5. 🚀 ¡Estarás listo para el curso!

---

**Última actualización:** Noviembre 2025 | **Versión:** 2.1
