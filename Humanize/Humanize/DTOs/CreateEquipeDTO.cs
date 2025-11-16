using System.ComponentModel.DataAnnotations;

namespace Humanize.DTOs
{
    public class CreateEquipeDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;
    }
}