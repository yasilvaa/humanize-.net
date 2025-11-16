using System.ComponentModel.DataAnnotations;

namespace Humanize.DTOs
{
    public class CreatePerguntaDTO
    {
        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(1000, ErrorMessage = "Título deve ter no máximo 1000 caracteres")]
        public string Titulo { get; set; } = string.Empty;
    }
}