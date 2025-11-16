namespace Humanize.Infrastructure.Persistence.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;

        public int EquipeId { get; set; }
        public int? VoucherId { get; set; }

        public virtual Equipe Equipe { get; set; } = null!;
        public virtual Voucher? Voucher { get; set; }
        public virtual ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
    }
}
