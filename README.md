# Guía de Instalación y Configuración

## Paso 1: Clonar el Repositorio
```bash
git clone https://github.com/AngelSt7/SOA-API.git
abrir el .SLN en VS
```

## Paso 2: Restaurar Paquetes NuGet
```bash
dotnet restore
```

## Paso 3: Levantar los Contenedores Docker
```bash
docker compose up -d
```

## Paso 4: Abrir la Consola del Administrador de Paquetes
1. Ir a **Ver** en el menú superior
2. Seleccionar **Otras ventanas**
3. Hacer clic en **Consola del Administrador de Paquetes**

## Paso 5: Aplicar la Migración Inicial
En la Consola del Administrador de Paquetes, ejecutar:
```powershell
Update-Database
```

## Paso 6: Ejecutar la Aplicación
Presionar el botón verde de **Play/Run** en Visual Studio

## Paso 7: Ejecutar el Seed de Datos
Realizar una petición GET a:
```
http://localhost:7026/api/seed
```