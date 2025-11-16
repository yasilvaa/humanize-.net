using Humanize.Infrastructure.Persistence.Entities;

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
    }
}
