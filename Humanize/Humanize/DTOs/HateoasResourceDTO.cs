namespace Humanize.DTOs
{
    public abstract class HateoasResourceDTO
    {
        public List<HateoasLinkDTO> Links { get; set; } = new();
    }
}