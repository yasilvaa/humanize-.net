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
