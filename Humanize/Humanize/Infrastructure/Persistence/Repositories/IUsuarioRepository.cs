using Humanize.Infrastructure.Persistence.Entities;

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
    }
}
