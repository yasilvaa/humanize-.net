namespace Humanize.DTOs
{
    public class VoucherDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Loja { get; set; } = string.Empty;
        public DateTime? Validade { get; set; }
        public string? Status { get; set; } 
        public bool IsVencido => Validade.HasValue && DateTime.Now > Validade.Value;
        public int TotalUsuarios { get; set; } 
    }
}