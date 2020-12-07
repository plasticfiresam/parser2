IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [ProductCategories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_ProductCategories] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Sellers] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Link] nvarchar(max) NULL,
    CONSTRAINT [PK_Sellers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Price] int NOT NULL,
    [Link] nvarchar(max) NULL,
    [Visitors] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Number] nvarchar(max) NULL,
    [SellerId] int NOT NULL,
    [ProductCategoryId] int NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_ProductCategories_ProductCategoryId] FOREIGN KEY ([ProductCategoryId]) REFERENCES [ProductCategories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Products_Sellers_SellerId] FOREIGN KEY ([SellerId]) REFERENCES [Sellers] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Products_ProductCategoryId] ON [Products] ([ProductCategoryId]);

GO

CREATE INDEX [IX_Products_SellerId] ON [Products] ([SellerId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201207212702_initial', N'3.1.10');

GO

