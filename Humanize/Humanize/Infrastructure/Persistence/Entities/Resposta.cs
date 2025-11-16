namespace Humanize.Infrastructure.Persistence.Entities
{
    public class Resposta
    {
        public int Id { get; set; }
        public int Humor { get; set; }
        public int Categoria { get; set; } 
        public string? Comentario { get; set; }
        
        public int AvaliacaoId { get; set; }
        public int PerguntaId { get; set; }
        
        public virtual Avaliacao Avaliacao { get; set; }
        public virtual Pergunta Pergunta { get; set; }
    }
}
