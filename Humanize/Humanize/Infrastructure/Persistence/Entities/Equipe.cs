namespace Humanize.Infrastructure.Persistence.Entities
{
    public class Equipe
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
