IF DB_ID(N'GestionProyectos') IS NULL
    CREATE DATABASE GestionProyectos;
GO

USE GestionProyectos;
GO

CREATE TABLE dbo.Roles (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(200) NULL,
    CONSTRAINT PK_Roles PRIMARY KEY (Id),
    CONSTRAINT UQ_Roles_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.Estados (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    EntidadAplicacion NVARCHAR(20) NOT NULL,
    CONSTRAINT PK_Estados PRIMARY KEY (Id),
    CONSTRAINT CK_Estados_Entidad CHECK (EntidadAplicacion IN (N'Proyecto', N'Tarea')),
    CONSTRAINT UQ_Estados_Nombre_Entidad UNIQUE (Nombre, EntidadAplicacion)
);
GO

CREATE TABLE dbo.Prioridades (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(20) NOT NULL,
    CONSTRAINT PK_Prioridades PRIMARY KEY (Id),
    CONSTRAINT UQ_Prioridades_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.Usuarios (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    ContrasenaHash NVARCHAR(500) NOT NULL,
    IdRol INT NOT NULL,
    FechaRegistro DATETIME2 NOT NULL CONSTRAINT DF_Usuarios_FechaRegistro DEFAULT SYSDATETIME(),
    Activo BIT NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT 1,
    CONSTRAINT PK_Usuarios PRIMARY KEY (Id),
    CONSTRAINT UQ_Usuarios_Email UNIQUE (Email),
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (IdRol) REFERENCES dbo.Roles (Id) ON DELETE NO ACTION
);
GO

CREATE TABLE dbo.Proyectos (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(1000) NULL,
    FechaInicio DATE NOT NULL,
    FechaFinEstimada DATE NOT NULL,
    IdEstado INT NOT NULL,
    IdResponsable INT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL CONSTRAINT DF_Proyectos_FechaCreacion DEFAULT SYSDATETIME(),
    CONSTRAINT PK_Proyectos PRIMARY KEY (Id),
    CONSTRAINT FK_Proyectos_Estados FOREIGN KEY (IdEstado) REFERENCES dbo.Estados (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Proyectos_Usuarios FOREIGN KEY (IdResponsable) REFERENCES dbo.Usuarios (Id) ON DELETE NO ACTION
);
GO

CREATE TABLE dbo.Tareas (
    Id INT IDENTITY(1,1) NOT NULL,
    IdProyecto INT NOT NULL,
    Titulo NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(1000) NULL,
    IdAsignado INT NOT NULL,
    IdEstado INT NOT NULL,
    IdPrioridad INT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL CONSTRAINT DF_Tareas_FechaCreacion DEFAULT SYSDATETIME(),
    FechaVencimiento DATE NOT NULL,
    CONSTRAINT PK_Tareas PRIMARY KEY (Id),
    CONSTRAINT FK_Tareas_Proyectos FOREIGN KEY (IdProyecto) REFERENCES dbo.Proyectos (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Tareas_Usuarios FOREIGN KEY (IdAsignado) REFERENCES dbo.Usuarios (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Tareas_Estados FOREIGN KEY (IdEstado) REFERENCES dbo.Estados (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Tareas_Prioridades FOREIGN KEY (IdPrioridad) REFERENCES dbo.Prioridades (Id) ON DELETE NO ACTION
);
GO

CREATE INDEX IX_Usuarios_IdRol ON dbo.Usuarios (IdRol);
CREATE INDEX IX_Proyectos_IdEstado ON dbo.Proyectos (IdEstado);
CREATE INDEX IX_Proyectos_IdResponsable ON dbo.Proyectos (IdResponsable);
CREATE INDEX IX_Tareas_IdProyecto_IdEstado ON dbo.Tareas (IdProyecto, IdEstado);
CREATE INDEX IX_Tareas_IdAsignado_IdEstado ON dbo.Tareas (IdAsignado, IdEstado);
CREATE INDEX IX_Tareas_IdEstado ON dbo.Tareas (IdEstado);
CREATE INDEX IX_Tareas_IdPrioridad ON dbo.Tareas (IdPrioridad);
GO
