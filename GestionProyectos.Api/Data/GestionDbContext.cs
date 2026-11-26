using GestionProyectos.Api.Data.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GestionProyectos.Api.Data;

public class GestionDbContext(DbContextOptions<GestionDbContext> options) : DbContext(options)
{
    public DbSet<Rol> Roles => Set<Rol>();

    public DbSet<Estado> Estados => Set<Estado>();

    public DbSet<Prioridad> Prioridades => Set<Prioridad>();

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Proyecto> Proyectos => Set<Proyecto>();

    public DbSet<Tarea> Tareas => Set<Tarea>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>(entidad =>
        {
            entidad.Property(r => r.Nombre).HasMaxLength(50);
            entidad.Property(r => r.Descripcion).HasMaxLength(200);
        });

        modelBuilder.Entity<Estado>(entidad =>
        {
            entidad.Property(e => e.Nombre).HasMaxLength(50);
            entidad.Property(e => e.EntidadAplicacion).HasMaxLength(20);
        });

        modelBuilder.Entity<Prioridad>(entidad =>
        {
            entidad.Property(p => p.Nombre).HasMaxLength(20);
        });

        modelBuilder.Entity<Usuario>(entidad =>
        {
            entidad.Property(u => u.Nombre).HasMaxLength(100);
            entidad.Property(u => u.Email).HasMaxLength(150);
            entidad.Property(u => u.ContrasenaHash).HasMaxLength(500);
            entidad.Property(u => u.FechaRegistro).HasDefaultValueSql("SYSDATETIME()");

            entidad.HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IdRol)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Proyecto>(entidad =>
        {
            entidad.Property(p => p.Nombre).HasMaxLength(150);
            entidad.Property(p => p.Descripcion).HasMaxLength(1000);
            entidad.Property(p => p.FechaCreacion).HasDefaultValueSql("SYSDATETIME()");

            entidad.HasOne(p => p.Estado)
                .WithMany(e => e.Proyectos)
                .HasForeignKey(p => p.IdEstado)
                .OnDelete(DeleteBehavior.NoAction);

            entidad.HasOne(p => p.Responsable)
                .WithMany(u => u.ProyectosResponsable)
                .HasForeignKey(p => p.IdResponsable)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Tarea>(entidad =>
        {
            entidad.Property(t => t.Titulo).HasMaxLength(150);
            entidad.Property(t => t.Descripcion).HasMaxLength(1000);
            entidad.Property(t => t.FechaCreacion).HasDefaultValueSql("SYSDATETIME()");

            entidad.HasOne(t => t.Proyecto)
                .WithMany(p => p.Tareas)
                .HasForeignKey(t => t.IdProyecto)
                .OnDelete(DeleteBehavior.NoAction);

            entidad.HasOne(t => t.Asignado)
                .WithMany(u => u.TareasAsignadas)
                .HasForeignKey(t => t.IdAsignado)
                .OnDelete(DeleteBehavior.NoAction);

            entidad.HasOne(t => t.Estado)
                .WithMany(e => e.Tareas)
                .HasForeignKey(t => t.IdEstado)
                .OnDelete(DeleteBehavior.NoAction);

            entidad.HasOne(t => t.Prioridad)
                .WithMany(p => p.Tareas)
                .HasForeignKey(t => t.IdPrioridad)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
