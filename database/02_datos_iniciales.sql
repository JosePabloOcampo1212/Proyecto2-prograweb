USE GestionProyectos;
GO

INSERT INTO dbo.Roles (Nombre, Descripcion) VALUES
    (N'Administrador', N'Lider o gestor de proyectos'),
    (N'Usuario', N'Miembro de equipo');

INSERT INTO dbo.Estados (Nombre, EntidadAplicacion) VALUES
    (N'Activo', N'Proyecto'),
    (N'Cerrado', N'Proyecto'),
    (N'Pendiente', N'Tarea'),
    (N'En progreso', N'Tarea'),
    (N'Completada', N'Tarea'),
    (N'Cancelada', N'Tarea');

INSERT INTO dbo.Prioridades (Nombre) VALUES
    (N'Alta'),
    (N'Media'),
    (N'Baja');

INSERT INTO dbo.Usuarios (Nombre, Email, ContrasenaHash, IdRol)
SELECT N'Administrador', N'admin@gestion.local',
       N'AQAAAAEAAYagAAAAEKfc/u1CQVSysHTJ3QhRWrdxKZkGIE121uBv6cyP5SXbqyPnesDSZIMppm25742vQA==',
       Id
FROM dbo.Roles
WHERE Nombre = N'Administrador';
GO
