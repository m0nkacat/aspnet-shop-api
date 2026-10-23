# 🔌 ASP.NET Shop REST API

![.NET](https://img.shields.io/badge/.NET_8-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core Web API](https://img.shields.io/badge/ASP.NET_Core_Web_API-512BD4?logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF_Core_9-68217A)
![AutoMapper](https://img.shields.io/badge/AutoMapper-BE161E)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=black)

**RU** | [EN](#english)

REST API интернет-магазина на ASP.NET Core с многослойной архитектурой: контроллеры → сервисы → репозитории → EF Core.

## Возможности
- 📦 CRUD для товаров, категорий, пользователей, ролей, заказов, корзин
- 🧱 Обобщённые `CrudController<T>` / `ICrudService<T>` / `IRepository<T>`: без дублирования кода
- 🔁 DTO и маппинг через AutoMapper
- 📄 Пагинация (`PagedResult<T>`)
- 📅 Бронирование товаров с отменой (`POST /api/reservations/{id}/cancel`)
- 📘 Документация Swagger UI

## Эндпоинты
`/api/products` · `/api/categories` · `/api/users` · `/api/roles` · `/api/orders` · `/api/order-items` · `/api/carts` · `/api/cart-items` · `/api/reservations`

## Стек
C# · .NET 8 · ASP.NET Core Web API · EF Core 9 (SQL Server) · AutoMapper · Swashbuckle

## Запуск
1. Укажи `connectionString` в `appsettings.json` (по умолчанию `localhost\SQLEXPRESS`, БД `ShopDb`); скрипт для таблицы бронирований лежит в `Database/AddReservations.sql`.
2. `dotnet run`, затем открой `/swagger`.

---

## English

A layered REST API for an online shop (controllers → services → repositories → EF Core).

**Features:** CRUD for all shop entities via generic controller/service/repository · DTOs with AutoMapper · pagination · product reservations with cancellation · Swagger UI.

**Run:** set `connectionString` in `appsettings.json`, run `dotnet run` and open `/swagger`.
