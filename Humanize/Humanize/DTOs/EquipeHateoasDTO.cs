namespace Humanize.DTOs
{
    public class EquipeHateoasDTO : HateoasResourceDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int TotalUsuarios { get; set; }
    }
}