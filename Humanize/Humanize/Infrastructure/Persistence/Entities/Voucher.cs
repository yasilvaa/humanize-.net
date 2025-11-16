namespace Humanize.Infrastructure.Persistence.Entities
{
    public class Voucher
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Loja { get; set; }
        public DateTime Validade { get; set; }
        public string Status { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
