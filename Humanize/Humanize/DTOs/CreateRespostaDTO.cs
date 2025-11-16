using System.ComponentModel.DataAnnotations;

namespace Humanize.DTOs
{
    public class CreateRespostaDTO
    {
        [Required(ErrorMessage = "Humor é obrigatório")]
        [Range(1, 5, ErrorMessage = "Humor deve ser de 1 e 5")]
        public int Humor { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "Categoria deve ser um número positivo")]
        public int Categoria { get; set; }

        [StringLength(1000, ErrorMessage = "Comentário deve ter no máximo 1000 caracteres")]
        public string? Comentario { get; set; }

        [Required(ErrorMessage = "ID da avaliação é obrigatório")]
        public int AvaliacaoId { get; set; }

        [Required(ErrorMessage = "ID da pergunta é obrigatório")]
        public int PerguntaId { get; set; }
    }
}