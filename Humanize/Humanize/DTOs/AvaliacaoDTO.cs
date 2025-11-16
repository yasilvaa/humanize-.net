namespace Humanize.DTOs
{
public class AvaliacaoDTO
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public int UsuarioId { get; set; }
    public string? UsuarioNome { get; set; }
    public int TotalRespostas { get; set; }
   }
}