using Microsoft.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;
using System.Linq.Expressions;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public class EquipeRepository : IEquipeRepository
    {
        private readonly HumanizeContext _context;

        public EquipeRepository(HumanizeContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Equipe> Data, int TotalCount)> SearchAsync(EquipeSearchParametersDTO parameters)
        {
            var query = _context.Equipes
                .Include(e => e.Usuarios)
                .AsQueryable();

            // Filtros
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(e => e.Nome.Contains(parameters.SearchTerm));
            }

            if (!string.IsNullOrEmpty(parameters.Nome))
            {
                query = query.Where(e => e.Nome.Contains(parameters.Nome));
            }

            if (parameters.MinUsuarios.HasValue)
            {
                query = query.Where(e => e.Usuarios.Count >= parameters.MinUsuarios.Value);
            }

            if (parameters.MaxUsuarios.HasValue)
            {
                query = query.Where(e => e.Usuarios.Count <= parameters.MaxUsuarios.Value);
            }

            var totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                var isDescending = parameters.SortDirection?.ToLower() == "desc";

                query = parameters.SortBy.ToLower() switch
                {
                    "id" => isDescending ? query.OrderByDescending(e => e.Id) : query.OrderBy(e => e.Id),
                    "nome" => isDescending ? query.OrderByDescending(e => e.Nome) : query.OrderBy(e => e.Nome),
                    "totalusuarios" => isDescending ? query.OrderByDescending(e => e.Usuarios.Count) : query.OrderBy(e => e.Usuarios.Count),
                    _ => query.OrderBy(e => e.Id)
                };
            }

            var data = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (data, totalCount);
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
