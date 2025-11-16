namespace Humanize.DTOs
{
    public class EquipeDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int TotalUsuarios { get; set; } // qtde de usuários na equipe
    }
}