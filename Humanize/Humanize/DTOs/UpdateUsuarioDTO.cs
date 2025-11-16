using System.ComponentModel.DataAnnotations;

namespace Humanize.DTOs
{
    public class UpdateUsuarioDTO
    {
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string? Nome { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }

        [MinLength(6, ErrorMessage = "Senha deve ter pelo menos 6 caracteres")]
        [StringLength(50, ErrorMessage = "Senha deve ter no máximo 50 caracteres")]
        public string? Senha { get; set; }

        [StringLength(11, ErrorMessage = "Tipo deve ter no máximo 11 caracteres")]
        public string? Tipo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ID da equipe deve ser um número positivo")]
        public int? EquipeId { get; set; }

        public int? VoucherId { get; set; }
    }
}