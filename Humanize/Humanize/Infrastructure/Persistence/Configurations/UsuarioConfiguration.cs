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
                .HasColumnName("id_usuario")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Nome)
                .HasColumnName("nm_usuario")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("em_usuario")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Tipo)
                .HasColumnName("tp_usuario")
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(u => u.Senha)
                .HasColumnName("senha_usuario")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.EquipeId)
                .HasColumnName("T_HUMANIZE_EQUIPE_id_equipe")
                .IsRequired();

            builder.Property(u => u.VoucherId)
                .HasColumnName("T_HUMANIZE_VOUCHER_id_voucher")
                .IsRequired();


            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("T_HUMANIZE_USUARIO_em_usuario_UN");

            // relacionamentos
            builder.HasOne(u => u.Equipe)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(u => u.EquipeId)
                .HasConstraintName("T_HUMANIZE_USUARIO_T_HUMANIZE_EQUIPE_FK")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Voucher)
                .WithMany(v => v.Usuarios)
                .HasForeignKey(u => u.VoucherId)
                .HasConstraintName("T_HUMANIZE_USUARIO_T_HUMANIZE_VOUCHER_FK")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}