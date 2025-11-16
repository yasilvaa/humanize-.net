namespace Humanize.DTOs
{
    public class VoucherDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Loja { get; set; } = string.Empty;
        public DateTime Validade { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsVencido => DateTime.Now > Validade;
        public int TotalUsuarios { get; set; } // qtde de usuários com este voucher
    }
}