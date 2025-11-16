namespace Humanize.DTOs
{
    public class RespostaDTO
    {
        public int Id { get; set; }
        public int Humor { get; set; }
        public int Categoria { get; set; }
        public string? Comentario { get; set; }
        public int AvaliacaoId { get; set; }
        public DateTime? AvaliacaoDataHora { get; set; }
        public int PerguntaId { get; set; }
        public string? PerguntaTitulo { get; set; } 
    }
}