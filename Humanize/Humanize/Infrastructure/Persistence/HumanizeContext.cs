using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence.Entities;
using Humanize.Infrastructure.Persistence.Configurations;

namespace Humanize.Infrastructure.Persistence
{
    public class HumanizeContext : DbContext
    {
        public HumanizeContext(DbContextOptions<HumanizeContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Pergunta> Perguntas { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Resposta> Respostas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HumanizeContext).Assembly);


            // tb Equipe
            modelBuilder.Entity<Equipe>(entity =>
             {
                 entity.ToTable("T_HUMANIZE_EQUIPE");
                 entity.HasKey(e => e.Id);
                 entity.Property(e => e.Id).HasColumnName("id_equipe").ValueGeneratedOnAdd();
                 entity.Property(e => e.Nome).HasColumnName("nm_equipe").HasMaxLength(100).IsRequired();
                 entity.HasIndex(e => e.Nome).IsUnique().HasDatabaseName("T_HUMANIZE_EQUIPE_nm_equipe_UN");
             });

            // tb Voucher
            modelBuilder.Entity<Voucher>(entity =>
             {
                 entity.ToTable("T_HUMANIZE_VOUCHER");
                 entity.HasKey(v => v.Id);
                 entity.Property(v => v.Id).HasColumnName("id_voucher").ValueGeneratedOnAdd();
                 entity.Property(v => v.Nome).HasColumnName("nm_voucher").HasMaxLength(100).IsRequired();
                 entity.Property(v => v.Loja).HasColumnName("lj_voucher").HasMaxLength(100).IsRequired();
                 entity.Property(v => v.Validade).HasColumnName("val_voucher").HasColumnType("DATE").IsRequired();
                 entity.Property(v => v.Status).HasColumnName("st_voucher").HasMaxLength(1).IsRequired();
             });

            // tb Pergunta
            modelBuilder.Entity<Pergunta>(entity =>
            {
                entity.ToTable("T_HUMANIZE_PERGUNTA");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("id_pergunta").ValueGeneratedOnAdd();
                entity.Property(p => p.Titulo).HasColumnName("tt_pergunta").HasMaxLength(1000).IsRequired();
            });

            // tb Avaliacao
            modelBuilder.Entity<Avaliacao>(entity =>
            {
                entity.ToTable("T_HUMANIZE_AVALIACAO");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id).HasColumnName("id_avaliacao").ValueGeneratedOnAdd();
                entity.Property(a => a.DataHora).HasColumnName("dh_avaliacao").HasColumnType("DATE").IsRequired();
                entity.Property(a => a.UsuarioId).HasColumnName("T_HUMANIZE_USUARIO_id_usuario").IsRequired();

                // relacionamento
                entity.HasOne(a => a.Usuario)
                    .WithMany(u => u.Avaliacoes)
                    .HasForeignKey(a => a.UsuarioId)
                    .HasConstraintName("T_HUMANIZE_AVALIACAO_T_HUMANIZE_USUARIO_FK")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // tb Resposta
            modelBuilder.Entity<Resposta>(entity =>
            {
                entity.ToTable("T_HUMANIZE_RESPOSTA");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).HasColumnName("id_resposta").ValueGeneratedOnAdd();
                entity.Property(r => r.Humor).HasColumnName("humor_resposta").IsRequired();
                entity.Property(r => r.Categoria).HasColumnName("cg_resposta").IsRequired();
                entity.Property(r => r.Comentario).HasColumnName("cm_resposta").HasMaxLength(1000);
                entity.Property(r => r.AvaliacaoId).HasColumnName("T_HUMANIZE_AVALIACAO_id_avaliacao").IsRequired();
                entity.Property(r => r.PerguntaId).HasColumnName("T_HUMANIZE_PERGUNTA_id_pergunta").IsRequired();

                // eelacionamentos
                entity.HasOne(r => r.Avaliacao)
                      .WithMany(a => a.Respostas)
                      .HasForeignKey(r => r.AvaliacaoId)
                      .HasConstraintName("T_HUMANIZE_RESPOSTA_T_HUMANIZE_AVALIACAO_FK")
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Pergunta)
                      .WithMany(p => p.Respostas)
                      .HasForeignKey(r => r.PerguntaId)
                      .HasConstraintName("T_HUMANIZE_RESPOSTA_T_HUMANIZE_PERGUNTA_FK")
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle("DefaultConnection");
            }
        }
    }
}
