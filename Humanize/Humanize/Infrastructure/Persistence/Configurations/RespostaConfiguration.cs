using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Humanize.Infrastructure.Persistence.Entities;

namespace Humanize.Infrastructure.Persistence.Configurations
{
    public class RespostaConfiguration : IEntityTypeConfiguration<Resposta>
    {
        public void Configure(EntityTypeBuilder<Resposta> builder)
        {
            builder.ToTable("T_HUMANIZE_RESPOSTA");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("ID_RESPOSTA")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Humor)
                .HasColumnName("HUMOR_RESPOSTA")
                .IsRequired();

            builder.Property(r => r.Categoria)
                .HasColumnName("CG_RESPOSTA")
                .IsRequired();

            builder.Property(r => r.Comentario)
                .HasColumnName("CM_RESPOSTA")
                .HasMaxLength(1000);

            builder.Property(r => r.AvaliacaoId)
                .HasColumnName("T_HUMANIZE_AVALIACAO_ID_AVALIACAO")
                .IsRequired();

            builder.Property(r => r.PerguntaId)
                .HasColumnName("T_HUMANIZE_PERGUNTA_ID_PERGUNTA")
                .IsRequired();

            // relacionamentos
            builder.HasOne(r => r.Avaliacao)
                .WithMany(a => a.Respostas)
                .HasForeignKey(r => r.AvaliacaoId)
                .HasConstraintName("FK_HUMANIZE_RESPOSTA_AVALIACAO")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Pergunta)
                .WithMany(p => p.Respostas)
                .HasForeignKey(r => r.PerguntaId)
                .HasConstraintName("FK_HUMANIZE_RESPOSTA_PERGUNTA")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}