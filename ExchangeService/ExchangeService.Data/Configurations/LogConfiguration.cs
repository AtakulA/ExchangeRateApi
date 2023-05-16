using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Data
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.MachineName).HasColumnType("nvarchar(50)");
            builder.Property(x => x.Logged).HasColumnType("DateTime");
            builder.Property(x => x.Level).HasColumnType("nvarchar(50)");
            builder.Property(x => x.Message).HasColumnType("nvarchar(max)");
            builder.Property(x => x.Logger).HasColumnType("nvarchar(max)");
            builder.Property(x => x.Exception).HasColumnType("nvarchar(max)");

            builder.ToTable("Logs");
        }
    }
}
