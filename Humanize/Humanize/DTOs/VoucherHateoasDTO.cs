namespace Humanize.DTOs
{
    public class VoucherHateoasDTO : HateoasResourceDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Loja { get; set; } = string.Empty;
        public DateTime? Validade { get; set; }
        public string? Status { get; set; }
        public int TotalUsuarios { get; set; }
    }
}