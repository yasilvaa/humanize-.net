using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Humanize.Infrastructure.Persistence.Entities;

namespace Humanize.Infrastructure.Persistence.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("T_HUMANIZE_USUARIO");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("ID_USUARIO")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Nome)
                 .HasColumnName("NM_USUARIO")
                 .HasMaxLength(100)
                 .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("EM_USUARIO")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Tipo)
                .HasColumnName("TP_USUARIO")
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(u => u.Senha)
                 .HasColumnName("SENHA_USUARIO")
                 .HasMaxLength(50)
                  .IsRequired();

            builder.Property(u => u.EquipeId)
                  .HasColumnName("T_HUMANIZE_EQUIPE_ID_EQUIPE")
                  .IsRequired();

            builder.Property(u => u.VoucherId)
                 .HasColumnName("T_HUMANIZE_VOUCHER_ID_VOUCHER")
                 .IsRequired(false);

            builder.HasIndex(u => u.Email)
                  .IsUnique()
                  .HasDatabaseName("UQ_HUMANIZE_EMAIL");

            // relacionamentos
            builder.HasOne(u => u.Equipe)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(u => u.EquipeId)
                .HasConstraintName("FK_HUMANIZE_USUARIO_EQUIPE")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Voucher)
               .WithMany(v => v.Usuarios)
               .HasForeignKey(u => u.VoucherId)
               .HasConstraintName("FK_HUMANIZE_USUARIO_VOUCHER")
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}