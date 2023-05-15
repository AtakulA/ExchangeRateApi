using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Data
{
    public class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
    {
        public void Configure(EntityTypeBuilder<ExchangeRate> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.From).HasColumnType("nvarchar(3)");
            builder.Property(x => x.To).HasColumnType("nvarchar(3)");
            builder.Property(x => x.Rate).HasColumnType("money");
            builder.Property(x => x.AcquiredAt).HasColumnType("DateTime");
            builder.Property(x => x.ExpiresAt).HasColumnType("DateTime");
            builder.Property(x => x.IsExpired).HasColumnType("bit");

            builder.ToTable("ExchangeRates");
        }
    }
}
