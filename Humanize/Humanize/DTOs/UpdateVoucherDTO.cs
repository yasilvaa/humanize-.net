using System.ComponentModel.DataAnnotations;

namespace Humanize.DTOs
{
    public class UpdateVoucherDTO
    {
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string? Nome { get; set; }

        [StringLength(100, ErrorMessage = "Loja deve ter no máximo 100 caracteres")]
        public string? Loja { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Validade { get; set; }

        [StringLength(1, ErrorMessage = "Status deve ter exatamente 1 caractere")]
        public string? Status { get; set; }
    }
}