using CryptoFlux.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoFlux.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Transaccion> Transacciones { get; set; }

    }
}
