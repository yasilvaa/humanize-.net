namespace Humanize.Infrastructure.Persistence.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public int EquipeId { get; set; }
        public int VoucherId { get; set; }

        public virtual Equipe Equipe { get; set; }
        public virtual Voucher Voucher { get; set; }
        public virtual ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();

    }
}
