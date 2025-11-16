using System.ComponentModel.DataAnnotations;

namespace Humanize.DTOs
{
    public class CreateAvaliacaoDTO
    {
        [Required(ErrorMessage = "ID do usuário é obrigatório")]
        public int UsuarioId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DataHora { get; set; } 
    }
}