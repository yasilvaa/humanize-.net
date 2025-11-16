namespace Humanize.DTOs
{
    public class PerguntaDTO
    {
      public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int TotalRespostas { get; set; } // qtde de respostas desta pergunta
    }
}