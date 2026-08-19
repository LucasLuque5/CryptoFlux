# CryptoFlux

Trabajo Final - Programacion III

Sistema de gestion de cartera de criptomonedas.

## Tecnologias

- Frontend: Vue.js 3 + Vite + Axios
- Backend: ASP.NET Core 8 (C#)
- Base de datos: SQL Server
- ORM: Entity Framework Core
- API externa: CriptoYa (cotizaciones en tiempo real)

## Como correr el proyecto

### Backend

1. Abrir la solucion `CryptoFlux/CryptoFlux.API` en Visual Studio 2022
2. Ejecutar el proyecto con F5
3. Swagger disponible en: `https://localhost:7212/swagger`

### Frontend

1. Abrir la carpeta `crypto-flux` en VS Code
2. Ejecutar en la terminal:

```bash
npm install
npm run dev
```

3. La app estara disponible en: `http://localhost:5173`

## Funcionalidades

- Registrar compras de criptomonedas (cotizacion automatica via CriptoYa)
- Registrar ventas de criptomonedas (cotizacion automatica + validacion de saldo)
- Historial de movimientos
- Ver, editar y eliminar transacciones
- Analisis de cartera con precios actuales
- Base de datos persistente (SQL Server)

## Autor

Lucas Luque - Legajo 17997
