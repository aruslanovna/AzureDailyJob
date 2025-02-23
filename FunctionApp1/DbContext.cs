using CurrencyExchange.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateKeeper
{
    public class CurrencyDbContext : DbContext
    {
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        private readonly IConfiguration _configuration;

        public CurrencyDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration["ConnectionStrings:ExchangeRateDb"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is missing.");
            }

            optionsBuilder.UseSqlServer(connectionString);
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
