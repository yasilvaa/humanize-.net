using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Humanize.Infrastructure.Persistence.Entities;

namespace Humanize.Infrastructure.Persistence.Configurations
{
    public class EquipeConfiguration : IEntityTypeConfiguration<Equipe>
    {
        public void Configure(EntityTypeBuilder<Equipe> builder)
        {
            builder.ToTable("T_HUMANIZE_EQUIPE");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("ID_EQUIPE")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Nome)
                .HasColumnName("NM_EQUIPE")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(e => e.Nome)
                .IsUnique()
                .HasDatabaseName("UQ_HUMANIZE_EQUIPE_NM");
        }
    }
}