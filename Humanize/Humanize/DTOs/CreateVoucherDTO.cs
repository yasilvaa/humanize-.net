using System.ComponentModel.DataAnnotations;

namespace Humanize.DTOs
{
    public class CreateVoucherDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Loja é obrigatória")]
        [StringLength(100, ErrorMessage = "Loja deve ter no máximo 100 caracteres")]
        public string Loja { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? Validade { get; set; }

        [StringLength(1, ErrorMessage = "Status deve ter exatamente 1 caractere")]
        public string? Status { get; set; }
    }
}