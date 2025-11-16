namespace Humanize.Infrastructure.Persistence.Entities
{
    public class Equipe
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
