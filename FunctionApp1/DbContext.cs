using CurrencyExchange.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateKeeper
{

    public class CurrencyDbContext : DbContext
    {
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:exchangerate.database.windows.net,1433;Initial Catalog=ExchangeRate;Persist Security Info=False;User ID=anastasiia;Password=viSSevaSSe47.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeRate>()
              .HasOne(er => er.CurrencyRate)
              .WithMany(cr => cr.Rates)
              .HasForeignKey(er => er.CurrencyRateId);
        }
    }
}
