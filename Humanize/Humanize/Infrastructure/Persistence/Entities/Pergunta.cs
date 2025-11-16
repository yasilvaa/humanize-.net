namespace Humanize.Infrastructure.Persistence.Entities
{
    public class Pergunta
    {
        public int Id { get; set; }
        public string Titulo { get; set; }

        public virtual ICollection<Resposta> Respostas { get; set; } = new List<Resposta>();
    }
}
