USE ShopDb;
GO

IF COL_LENGTH('dbo.Products', 'created_at') IS NULL
    ALTER TABLE dbo.Products
        ADD created_at DATETIME2 NOT NULL CONSTRAINT DF_Products_created_at DEFAULT SYSUTCDATETIME();
GO

IF COL_LENGTH('dbo.Products', 'stock') IS NULL
    ALTER TABLE dbo.Products
        ADD stock INT NOT NULL CONSTRAINT DF_Products_stock DEFAULT 10;
GO

IF OBJECT_ID('dbo.Reservations', 'U') IS NULL
    CREATE TABLE dbo.Reservations
    (
        reservation_id INT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_Reservations PRIMARY KEY,
        product_id     INT       NOT NULL CONSTRAINT FK_Reservations_Products REFERENCES dbo.Products (product_id),
        user_id        INT       NOT NULL CONSTRAINT FK_Reservations_Users REFERENCES dbo.Users (user_id),
        quantity       INT       NOT NULL CONSTRAINT CK_Reservations_quantity CHECK (quantity > 0),
        reserved_at    DATETIME2 NOT NULL,
        canceled_at    DATETIME2 NULL
    );
GO
