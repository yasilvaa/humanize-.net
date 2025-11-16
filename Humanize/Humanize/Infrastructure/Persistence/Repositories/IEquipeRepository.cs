using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public interface IEquipeRepository
    {
        Task<Equipe> AddAsync(Equipe equipe);
        Task<Equipe> GetByIdAsync(int id);
        Task<Equipe> GetByNomeAsync(string nome);
        Task<IEnumerable<Equipe>> GetAllAsync();
        Task UpdateAsync(Equipe equipe);
        Task DeleteAsync(int id);
        Task<bool> NomeExistsAsync(string nome);
        Task<IEnumerable<Equipe>> GetEquipesWithUsuariosAsync();
        Task<(IEnumerable<Equipe> Data, int TotalCount)> SearchAsync(EquipeSearchParametersDTO parameters);
    }
}
