using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Humanize.Infrastructure.Persistence.Entities;

namespace Humanize.Infrastructure.Persistence.Configurations
{
    public class AvaliacaoConfiguration : IEntityTypeConfiguration<Avaliacao>
    {
        public void Configure(EntityTypeBuilder<Avaliacao> builder)
        {
            builder.ToTable("T_HUMANIZE_AVALIACAO");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("ID_AVALIACAO")
                .ValueGeneratedOnAdd();

            builder.Property(a => a.DataHora)
                .HasColumnName("DH_AVALIACAO")
                .HasColumnType("DATE")
                .IsRequired();

            builder.Property(a => a.UsuarioId)
                .HasColumnName("T_HUMANIZE_USUARIO_ID_USUARIO")
                .IsRequired();

            // relacionamento
            builder.HasOne(a => a.Usuario)
              .WithMany(u => u.Avaliacoes)
              .HasForeignKey(a => a.UsuarioId)
              .HasConstraintName("FK_HUMANIZE_AVALIACAO_USUARIO")
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}