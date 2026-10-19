IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Clientes] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(100) NOT NULL,
    [Email] nvarchar(254) NOT NULL,
    [FechaNacimiento] date NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
);

CREATE TABLE [Editoriales] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Editoriales] PRIMARY KEY ([Id])
);

CREATE TABLE [Libros] (
    [Id] int NOT NULL IDENTITY,
    [Titulo] nvarchar(200) NOT NULL,
    [Isbn] nvarchar(13) NOT NULL,
    [Precio] decimal(12,2) NOT NULL,
    [EditorialId] int NOT NULL,
    CONSTRAINT [PK_Libros] PRIMARY KEY ([Id]),
    CONSTRAINT [CK_Libros_Precio] CHECK ([Precio] > 0),
    CONSTRAINT [FK_Libros_Editoriales_EditorialId] FOREIGN KEY ([EditorialId]) REFERENCES [Editoriales] ([Id]) ON DELETE NO ACTION
);

CREATE UNIQUE INDEX [IX_Clientes_Email] ON [Clientes] ([Email]);

CREATE INDEX [IX_Libros_EditorialId] ON [Libros] ([EditorialId]);

CREATE UNIQUE INDEX [IX_Libros_Isbn] ON [Libros] ([Isbn]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261019105823_CrearLibreria', N'10.0.13');

COMMIT;
GO

