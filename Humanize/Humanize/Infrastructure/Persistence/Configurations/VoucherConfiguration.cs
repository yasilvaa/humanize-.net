using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Humanize.Infrastructure.Persistence.Entities;

namespace Humanize.Infrastructure.Persistence.Configurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("T_HUMANIZE_VOUCHER");
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasColumnName("ID_VOUCHER") 
                .ValueGeneratedOnAdd();

            builder.Property(v => v.Nome)
                .HasColumnName("NM_VOUCHER") 
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(v => v.Loja)
                .HasColumnName("LJ_VOUCHER") 
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(v => v.Validade)
                .HasColumnName("VAL_VOUCHER") 
                .HasColumnType("DATE")
                .IsRequired(false);

            builder.Property(v => v.Status)
                .HasColumnName("ST_VOUCHER") 
                .HasMaxLength(1)
                .IsRequired(false);
        }
    }
}