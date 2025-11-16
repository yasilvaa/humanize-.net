using Microsoft.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence.Entities;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public class EquipeRepository : IEquipeRepository
    {
        private readonly HumanizeContext _context;

        public EquipeRepository(HumanizeContext context)
        {
            _context = context;
        }

        public async Task<Equipe> AddAsync(Equipe equipe)
        {
            _context.Equipes.Add(equipe);
            await _context.SaveChangesAsync();
            return equipe;
        }

        public async Task<Equipe> GetByIdAsync(int id)
        {
            return await _context.Equipes
                .Include(e => e.Usuarios)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Equipe> GetByNomeAsync(string nome)
        {
            return await _context.Equipes
                .Include(e => e.Usuarios)
                .FirstOrDefaultAsync(e => e.Nome == nome);
        }

        public async Task<IEnumerable<Equipe>> GetAllAsync()
        {
            return await _context.Equipes
                .Include(e => e.Usuarios)
                .ToListAsync();
        }

        public async Task UpdateAsync(Equipe equipe)
        {
            _context.Entry(equipe).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var equipe = await _context.Equipes.FindAsync(id);
            if (equipe != null)
            {
                _context.Equipes.Remove(equipe);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> NomeExistsAsync(string nome)
        {
            // Usar Count ao invés de AnyAsync para evitar problemas com Oracle
            var count = await _context.Equipes.CountAsync(e => e.Nome == nome);
            return count > 0;
        }

        public async Task<IEnumerable<Equipe>> GetEquipesWithUsuariosAsync()
        {
            // Usar Count ao invés de Any para evitar problemas com Oracle
            return await _context.Equipes
                .Include(e => e.Usuarios)
                .Where(e => e.Usuarios.Count() > 0)
                .ToListAsync();
        }
    }
}
