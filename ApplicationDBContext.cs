using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zonal_mvc.Models;

namespace Zonal_mvc
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<AfiliadosModel> AspNetAfiliados { get; set; }
        public DbSet<AmortizacionesModel> AspNetAmortizaciones { get; set; }
        public DbSet<PrestamosModel> AspNetPrestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new UtcDateTimeConverter());
                    }
                }
            }
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey("LoginProvider", "ProviderKey");
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey("UserId", "RoleId");  // Agregar esta línea
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey("UserId", "LoginProvider", "Name");  // Agregar esta línea

            modelBuilder.Entity<AmortizacionesModel>()
                .HasOne(a => a.Prestamo)
                .WithMany(p => p.Amortizaciones)
                .HasForeignKey(a => a.PrestamoID);

            // Otros ajustes de modelo si es necesario
        }

    }
}
