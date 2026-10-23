# aspnet-shop-api

REST API интернет-магазина на ASP.NET Core · *Online shop REST API on ASP.NET Core*

`C#` `.NET 8` `ASP.NET Core Web API` `EF Core 9` `SQL Server` `AutoMapper` `Swagger`

## Что сделано
- **Многослойная архитектура:** Controller → Service → Repository → EF Core
- **Обобщённый CRUD без дублирования:** `CrudController<TRead, TCreate, TUpdate, TPatch>`, `CrudService<…>` и `Repository<T>`. Контроллер конкретной сущности занимает одну строку
- **Полный набор HTTP-методов:** `GET` (список и по id), `POST`, `PUT`, `PATCH` (частичное обновление), `DELETE`
- **Пагинация:** параметры `page` и `pageSize` (1–100) с валидацией, общее количество записей отдаётся в заголовке `X-Total-Count`
- **DTO** для чтения, создания, обновления и патча у каждой сущности, маппинг через AutoMapper, валидация через `[Required]`, `[MaxLength]`, `[Range]`
- **`ServiceResult`:** сервис возвращает статус (`NotFound`, `Conflict`, `Invalid`), а базовый контроллер превращает его в правильный HTTP-ответ (404, 409, 400)
- **Бронирование товаров:** проверяется наличие на складе и активность пользователя, остаток списывается при бронировании и возвращается при отмене (`POST /api/reservations/{id}/cancel`)
- **Пароли** пользователей хранятся в виде SHA-256-хеша
- **Swagger UI** для документации и ручного тестирования

## Эндпоинты
| Ресурс | Путь |
|---|---|
| Товары | `/api/products` |
| Категории | `/api/categories` |
| Пользователи | `/api/users` |
| Роли | `/api/roles` |
| Заказы и позиции | `/api/orders`, `/api/order-items` |
| Корзины и позиции | `/api/carts`, `/api/cart-items` |
| Бронирования | `/api/reservations`, `POST /api/reservations/{id}/cancel` |

## Структура
```
Controllers/
├── ApiControllerBase.cs    # общая валидация пагинации, X-Total-Count, ServiceResult → HTTP-ответ
├── CrudController.cs       # обобщённый CRUD-контроллер
├── EntityControllers.cs    # контроллеры сущностей (по одной строке)
├── ProductsController.cs
└── ReservationsController.cs
Services/                   # ICrudService/CrudService, сервисы сущностей, ReservationService, ServiceResult
Repositories/               # IRepository/Repository (пагинация, AsNoTracking), репозитории сущностей
DTOs/                       # DTO для каждой сущности, PagedResult
Mapping/MappingProfile.cs   # правила AutoMapper
Models/                     # сущности EF Core, ShopDbContext
Database/AddReservations.sql  # миграция: таблица Reservations, поля stock и created_at
```

## Запуск
1. Укажи `connectionString` в `appsettings.json` (по умолчанию `localhost\SQLEXPRESS`, БД `ShopDb`) и выполни `Database/AddReservations.sql`.
2. Запусти:
   ```bash
   dotnet run
   ```
3. Открой `/swagger`.

---

## English

A layered REST API for an online shop built with ASP.NET Core.

### Features
- **Layered architecture**: Controller → Service → Repository → EF Core
- **Generic CRUD with no duplication**: `CrudController<TRead, TCreate, TUpdate, TPatch>`, `CrudService<…>`, `Repository<T>`; each entity controller is a single line
- **Full HTTP verb set**: `GET` (list / by id), `POST`, `PUT`, `PATCH` (partial update), `DELETE`
- **Pagination**: validated `page` / `pageSize` (1–100), total count in the `X-Total-Count` header
- **DTOs** for read/create/update/patch per entity, AutoMapper mapping, `[Required]` / `[MaxLength]` / `[Range]` validation
- **`ServiceResult`**: services return a status (`NotFound`, `Conflict`, `Invalid`) that the base controller turns into the right HTTP response (404 / 409 / 400)
- **Product reservations**: stock and user-active checks; stock is reduced on reserve and restored on cancel (`POST /api/reservations/{id}/cancel`)
- **Passwords** stored as SHA-256 hashes
- **Swagger UI** for docs and manual testing

### Endpoints
`/api/products` · `/api/categories` · `/api/users` · `/api/roles` · `/api/orders` · `/api/order-items` · `/api/carts` · `/api/cart-items` · `/api/reservations`

### Structure
```
Controllers/   base controller, generic CRUD controller, entity controllers, reservations
Services/      generic CRUD service, entity services, ReservationService, ServiceResult
Repositories/  generic repository (paging, AsNoTracking) and entity repositories
DTOs/          per-entity DTOs, PagedResult
Mapping/       AutoMapper profile
Models/        EF Core entities, ShopDbContext
Database/      SQL migration for reservations
```

### Run
1. Set `connectionString` in `appsettings.json` (default `localhost\SQLEXPRESS`, database `ShopDb`) and run `Database/AddReservations.sql`.
2. `dotnet run`, then open `/swagger`.
