USE ShopDb;
GO

IF EXISTS (SELECT 1 FROM sys.columns
           WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'password_hash' AND max_length < 512)
    ALTER TABLE dbo.Users ALTER COLUMN password_hash NVARCHAR(256) NULL;
GO
