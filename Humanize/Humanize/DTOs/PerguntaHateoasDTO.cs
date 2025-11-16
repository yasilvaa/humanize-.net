namespace Humanize.DTOs
{
    public class PerguntaHateoasDTO : HateoasResourceDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int TotalRespostas { get; set; }
    }
}