namespace Humanize.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int EquipeId { get; set; }
        public string? EquipeNome { get; set; } 
        public int VoucherId { get; set; }
        public string? VoucherNome { get; set; }
    }
}