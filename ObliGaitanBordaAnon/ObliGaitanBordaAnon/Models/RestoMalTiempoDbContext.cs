using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ObliGaitanBordaAnon.Models;

public partial class RestoMalTiempoDbContext : DbContext
{
    public RestoMalTiempoDbContext()
    {
    }

    public RestoMalTiempoDbContext(DbContextOptions<RestoMalTiempoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Clima> Climas { get; set; }

    public virtual DbSet<Cotizacione> Cotizaciones { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<OrdenDetalle> OrdenDetalles { get; set; }

    public virtual DbSet<Ordene> Ordenes { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Resenia> Resenias { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Restaurante> Restaurantes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolesPermiso> RolesPermisos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkClientes");

            entity.HasIndex(e => e.Email, "uqClientesEmail").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.TipoCliente)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("tipoCliente");
        });

        modelBuilder.Entity<Clima>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkClima");

            entity.ToTable("Clima");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DescripcionClima)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcionClima");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Lluvia).HasColumnName("lluvia");
            entity.Property(e => e.Temperatura).HasColumnName("temperatura");
        });

        modelBuilder.Entity<Cotizacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkCotizaciones");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CotizacionDivisa).HasColumnName("cotizacionDivisa");
            entity.Property(e => e.NombreDivisa)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreDivisa");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkMenu");

            entity.ToTable("Menu");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("imagenUrl");
            entity.Property(e => e.NombrePlato)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombrePlato");
            entity.Property(e => e.Precio).HasColumnName("precio");
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkMesas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Capacidad).HasColumnName("capacidad");
            entity.Property(e => e.Estado)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.NumeroMesa).HasColumnName("numeroMesa");
            entity.Property(e => e.RestauranteId).HasColumnName("restauranteId");

            entity.HasOne(d => d.Restaurante).WithMany(p => p.Mesas)
                .HasForeignKey(d => d.RestauranteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fkMesasRest");
        });

        modelBuilder.Entity<OrdenDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkOrdenDetalles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.MenuId).HasColumnName("menuId");
            entity.Property(e => e.OrdenId).HasColumnName("ordenId");

            entity.HasOne(d => d.Menu).WithMany(p => p.OrdenDetalles)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk2OrdenDetalles");

            entity.HasOne(d => d.Orden).WithMany(p => p.OrdenDetalles)
                .HasForeignKey(d => d.OrdenId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk1OrdenDetalles");
        });

        modelBuilder.Entity<Ordene>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkOrdenes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ReservaId).HasColumnName("reservaId");
            entity.Property(e => e.Total).HasColumnName("total");

            entity.HasOne(d => d.Reserva).WithMany(p => p.Ordenes)
                .HasForeignKey(d => d.ReservaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fkOrdenes");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkPagos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClimaId).HasColumnName("climaId");
            entity.Property(e => e.CotizacionId).HasColumnName("cotizacionId");
            entity.Property(e => e.FechaPago)
                .HasColumnType("datetime")
                .HasColumnName("fechaPago");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("metodoPago");
            entity.Property(e => e.Monto).HasColumnName("monto");
            entity.Property(e => e.OrdenDetalleId).HasColumnName("ordenDetalleId");
            entity.Property(e => e.ReservaId).HasColumnName("reservaId");

            entity.HasOne(d => d.Clima).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.ClimaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk2Pagos");

            entity.HasOne(d => d.Cotizacion).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.CotizacionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk3Pagos");

            entity.HasOne(d => d.OrdenDetalle).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.OrdenDetalleId)
                .HasConstraintName("fk4Pagos");

            entity.HasOne(d => d.Reserva).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.ReservaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk1Pagos");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkPermisos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Resenia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkResenias");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteId).HasColumnName("clienteId");
            entity.Property(e => e.Comentario)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("comentario");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FechaResenia)
                .HasColumnType("datetime")
                .HasColumnName("fechaResenia");
            entity.Property(e => e.Puntaje).HasColumnName("puntaje");
            entity.Property(e => e.RestauranteId).HasColumnName("restauranteId");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Resenia)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk1Resenias");

            entity.HasOne(d => d.Restaurante).WithMany(p => p.Resenia)
                .HasForeignKey(d => d.RestauranteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk2Resenias");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkReservas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteId).HasColumnName("clienteId");
            entity.Property(e => e.Estado)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaReservada)
                .HasColumnType("datetime")
                .HasColumnName("fechaReservada");
            entity.Property(e => e.MesaId).HasColumnName("mesaId");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.RestauranteId).HasColumnName("restauranteId");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("fk1Reservas");

            entity.HasOne(d => d.Mesa).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.MesaId)
                .HasConstraintName("fk2Reservas");

            entity.HasOne(d => d.Restaurante).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.RestauranteId)
                .HasConstraintName("fk3Reservas");
        });

        modelBuilder.Entity<Restaurante>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkRestaurantes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkRoles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<RolesPermiso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkRolesPermisos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PermisoId).HasColumnName("permisoId");
            entity.Property(e => e.RolId).HasColumnName("rolId");

            entity.HasOne(d => d.Permiso).WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.PermisoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk2RolesPermisos");

            entity.HasOne(d => d.Rol).WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk1RolesPermisos");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkUsuarios");

            entity.HasIndex(e => e.Email, "uqUsuariosEmail").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fkUsuarios");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
