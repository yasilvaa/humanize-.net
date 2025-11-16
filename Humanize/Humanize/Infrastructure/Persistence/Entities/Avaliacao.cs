namespace Humanize.Infrastructure.Persistence.Entities
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        
        public int UsuarioId { get; set; }
   

        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Resposta> Respostas { get; set; } = new List<Resposta>();
   
        public Avaliacao()
        {
            DataHora = DateTime.Now;
        }
    }
}
