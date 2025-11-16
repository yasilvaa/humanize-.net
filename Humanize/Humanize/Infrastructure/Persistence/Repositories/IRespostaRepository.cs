using Humanize.Infrastructure.Persistence.Entities;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public interface IRespostaRepository
    {
        Task<Resposta> AddAsync(Resposta resposta);
        Task<Resposta> GetByIdAsync(int id);
        Task<IEnumerable<Resposta>> GetAllAsync();
        Task<IEnumerable<Resposta>> GetByAvaliacaoIdAsync(int avaliacaoId);
        Task<IEnumerable<Resposta>> GetByPerguntaIdAsync(int perguntaId);
        Task<IEnumerable<Resposta>> GetByHumorRangeAsync(int humorMin, int humorMax);
        Task<IEnumerable<Resposta>> GetByCategoriaAsync(int categoria);
        Task UpdateAsync(Resposta resposta);
        Task DeleteAsync(int id);
        Task<double> GetMediaHumorByPerguntaAsync(int perguntaId);
        Task<IEnumerable<Resposta>> GetRespostasWithComentarioAsync();
    }
}
