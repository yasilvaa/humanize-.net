using System.ComponentModel.DataAnnotations;

namespace Humanize.DTOs
{
    public class CreateUsuarioDTO
    {
        // regras de validação
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty; // string.Empty para que não haja valor null

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha deve ter pelo menos 6 caracteres")]
        [StringLength(50, ErrorMessage = "Senha deve ter no máximo 50 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo é obrigatório")]
        [StringLength(11, ErrorMessage = "Tipo deve ter no máximo 11 caracteres")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "ID da equipe é obrigatório")]
        public int EquipeId { get; set; }
        public int? VoucherId { get; set; }
    }

}
