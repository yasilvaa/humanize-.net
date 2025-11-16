using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public interface IUsuarioRepository
    {
        Task <Usuario> AddAsync(Usuario usuario);
        Task<Usuario> GetByIdAsync(int id);
        Task<Usuario> GetByEmailAsync(string email);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<(IEnumerable<Usuario> Data, int TotalCount)> SearchAsync(UsuarioSearchParametersDTO parameters);
    }
}
