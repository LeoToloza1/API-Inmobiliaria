using Microsoft.EntityFrameworkCore;

namespace inmobiliaria.Models
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Propietario> Propietario { get; set; }

        public DbSet<Inmueble> Inmueble { get; set; }
        public DbSet<Contrato> Contrato { get; set; }
        public DbSet<Inquilino> Inquilino { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Pago> Pago { get; set; }
        public DbSet<TipoInmueble> TipoInmueble { get; set; }
        /*
        Uso este metodo para convertir los enums a string, sino da este error:
        System.FormatException: The input string 'Disponible' was not in a correct format.
        */
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Inmueble>()
        //         .Property(e => e.estado)
        //         .HasConversion<string>();

        //     modelBuilder.Entity<Inmueble>()
        //         .Property(u => u.uso)
        //         .HasConversion<string>();
        // }
        //TODO buscar otra manera de hacerlo
    }

}

