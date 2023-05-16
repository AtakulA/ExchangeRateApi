using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Data
{
    public class ExchangeRatesDbContext : DbContext
    {
        public ExchangeRatesDbContext(DbContextOptions<ExchangeRatesDbContext> options) : base(options)
        {
        }

        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<ExchangeTrade> ExchangeTrades { get; set; }
        public DbSet<Log> Logs { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ExchangeRateConfiguration());
            builder.ApplyConfiguration(new ExchangeTradeConfiguration());
            builder.ApplyConfiguration(new LogConfiguration());
        }
    }
}
