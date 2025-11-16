using System.ComponentModel.DataAnnotations;

namespace Humanize.DTOs
{
    public class UpdateEquipeDTO
    {
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string? Nome { get; set; }
    }
}