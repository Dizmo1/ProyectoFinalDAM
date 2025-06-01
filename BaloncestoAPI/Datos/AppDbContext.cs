using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace BaloncestoAPI.Datos
{
    public class AppDbContext : DbContext
    {
        // Constructor mejorado con verificación nula
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets con inicialización para evitar warnings
        public DbSet<Jugador> Jugadores { get; set; } = null!;
        public DbSet<Partida> Partidas { get; set; } = null!;
        public DbSet<Tiro> Tiros { get; set; } = null!;
        public DbSet<Estadistica> Estadisticas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ¡No olvides esto!

            // Forzar nombres de tablas explícitos (evita cambios inesperados)
            modelBuilder.Entity<Jugador>().ToTable("Jugadores");
            modelBuilder.Entity<Partida>().ToTable("Partidas");
            modelBuilder.Entity<Tiro>().ToTable("Tiros");
            modelBuilder.Entity<Estadistica>().ToTable("Estadisticas");

            // Configuración de relaciones mejorada
            modelBuilder.Entity<Jugador>(entity =>
            {
                entity.HasMany(j => j.Partidas)
                      .WithOne(p => p.Jugador)
                      .HasForeignKey(p => p.JugadorId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(j => j.Email)
                      .IsUnique()
                      .HasDatabaseName("IX_Jugadores_Email");
            });

            modelBuilder.Entity<Partida>(entity =>
            {
                entity.HasIndex(p => new { p.JugadorId, p.Fecha })
                      .HasDatabaseName("IX_Partidas_JugadorFecha");
            });

            // Configuración explícita de tipos decimales
            modelBuilder.Entity<Tiro>(entity =>
            {
                entity.Property(t => t.Distancia)
                      .HasPrecision(10, 2);

                entity.Property(t => t.TiempoSegundos)
                      .HasPrecision(10, 2);
            });

            // Configuración de herencia para DateTime
            modelBuilder.Entity<Jugador>()
                .Property(j => j.FechaRegistro)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
