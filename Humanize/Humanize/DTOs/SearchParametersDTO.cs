namespace Humanize.DTOs
{
    public class SearchParametersDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "Id";
        public string? SortDirection { get; set; } = "asc"; // pode ser "asc" ou "desc"
        public string? SearchTerm { get; set; }
    }

    public class EquipeSearchParametersDTO : SearchParametersDTO
    {
        public string? Nome { get; set; }
        public int? MinUsuarios { get; set; }
        public int? MaxUsuarios { get; set; }
    }

    public class UsuarioSearchParametersDTO : SearchParametersDTO
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Tipo { get; set; }
        public int? EquipeId { get; set; }
        public int? VoucherId { get; set; }
        public bool? HasVoucher { get; set; }
    }

    public class VoucherSearchParametersDTO : SearchParametersDTO
    {
        public string? Nome { get; set; }
        public string? Loja { get; set; }
        public string? Status { get; set; }
        public DateTime? ValidadeInicio { get; set; }
        public DateTime? ValidadeFim { get; set; }
        public bool? IsVencido { get; set; }
        public int? MinUsuarios { get; set; }
        public int? MaxUsuarios { get; set; }
    }

    public class AvaliacaoSearchParametersDTO : SearchParametersDTO
    {
        public int? UsuarioId { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int? MinRespostas { get; set; }
        public int? MaxRespostas { get; set; }
    }

    public class PerguntaSearchParametersDTO : SearchParametersDTO
    {
        public string? Titulo { get; set; }
        public int? MinRespostas { get; set; }
        public int? MaxRespostas { get; set; }
        public bool? HasRespostas { get; set; }
    }

    public class RespostaSearchParametersDTO : SearchParametersDTO
    {
        public int? AvaliacaoId { get; set; }
        public int? PerguntaId { get; set; }
        public int? MinHumor { get; set; }
        public int? MaxHumor { get; set; }
        public int? Categoria { get; set; }
        public bool? HasComentario { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}