using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Humanize.Infrastructure.Persistence.Entities;

namespace Humanize.Infrastructure.Persistence.Configurations
{
    public class PerguntaConfiguration : IEntityTypeConfiguration<Pergunta>
    {
        public void Configure(EntityTypeBuilder<Pergunta> builder)
        {
            builder.ToTable("T_HUMANIZE_PERGUNTA");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
              .HasColumnName("ID_PERGUNTA")
              .ValueGeneratedOnAdd();

            builder.Property(p => p.Titulo)
              .HasColumnName("TT_PERGUNTA")
              .HasMaxLength(1000)
              .IsRequired();
        }
    }
}