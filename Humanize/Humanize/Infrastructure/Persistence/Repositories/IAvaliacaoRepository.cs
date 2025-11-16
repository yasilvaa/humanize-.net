using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public interface IAvaliacaoRepository
    {
        Task<Avaliacao> AddAsync(Avaliacao avaliacao);
        Task<Avaliacao> GetByIdAsync(int id);
        Task<IEnumerable<Avaliacao>> GetAllAsync();
        Task<IEnumerable<Avaliacao>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Avaliacao>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task UpdateAsync(Avaliacao avaliacao);
        Task DeleteAsync(int id);
        Task<IEnumerable<Avaliacao>> GetAvaliacoesWithRespostasAsync();
        Task<Avaliacao> GetUltimaAvaliacaoUsuarioAsync(int usuarioId);
        Task<(IEnumerable<Avaliacao> Data, int TotalCount)> SearchAsync(AvaliacaoSearchParametersDTO parameters);
    }
}
