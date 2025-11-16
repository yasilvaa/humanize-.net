using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public interface IPerguntaRepository
    {
        Task<Pergunta> AddAsync(Pergunta pergunta);
        Task<Pergunta> GetByIdAsync(int id);
        Task<IEnumerable<Pergunta>> GetAllAsync();
        Task UpdateAsync(Pergunta pergunta);
        Task DeleteAsync(int id);
        Task<IEnumerable<Pergunta>> GetPerguntasWithRespostasAsync();
        Task<IEnumerable<Pergunta>> SearchByTituloAsync(string titulo);
        Task<(IEnumerable<Pergunta> Data, int TotalCount)> SearchAsync(PerguntaSearchParametersDTO parameters);
    }
}
