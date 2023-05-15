using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Data
{
    public class ExchangeTradeConfiguration : IEntityTypeConfiguration<ExchangeTrade>
    {
        public void Configure(EntityTypeBuilder<ExchangeTrade> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).HasColumnType("int");
            builder.Property(x => x.ExchangeRateId).HasColumnType("int");
            builder.Property(x => x.UserAmount).HasColumnType("money");
            builder.Property(x => x.ExchangedAmount).HasColumnType("money");
            builder.Property(x => x.ExchangedAt).HasColumnType("DateTime");

            builder.HasOne(x=>x.ExchangeRate).WithMany(x=>x.ExchangeTrades).HasForeignKey(x=>x.ExchangeRateId);

            builder.ToTable("ExchangeTrades");
        }
    }
}
