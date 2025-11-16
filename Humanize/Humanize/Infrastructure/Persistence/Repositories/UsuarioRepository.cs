using Humanize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Humanize.DTOs;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly HumanizeContext _context;

        public UsuarioRepository(HumanizeContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Usuario> Data, int TotalCount)> SearchAsync(UsuarioSearchParametersDTO parameters)
        {
            var query = _context.Usuarios
                .Include(u => u.Equipe)
                .Include(u => u.Voucher)
                .Include(u => u.Avaliacoes)
                .AsQueryable();

            // Filtros
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(u => u.Nome.Contains(parameters.SearchTerm) ||
                 u.Email.Contains(parameters.SearchTerm) ||
                   u.Tipo.Contains(parameters.SearchTerm));
            }

            if (!string.IsNullOrEmpty(parameters.Nome))
            {
                query = query.Where(u => u.Nome.Contains(parameters.Nome));
            }

            if (!string.IsNullOrEmpty(parameters.Email))
            {
                query = query.Where(u => u.Email.Contains(parameters.Email));
            }

            if (!string.IsNullOrEmpty(parameters.Tipo))
            {
                query = query.Where(u => u.Tipo == parameters.Tipo);
            }

            if (parameters.EquipeId.HasValue)
            {
                query = query.Where(u => u.EquipeId == parameters.EquipeId.Value);
            }

            if (parameters.VoucherId.HasValue)
            {
                query = query.Where(u => u.VoucherId == parameters.VoucherId.Value);
            }

            if (parameters.HasVoucher.HasValue)
            {
                if (parameters.HasVoucher.Value)
                {
                    query = query.Where(u => u.VoucherId.HasValue);
                }
                else
                {
                    query = query.Where(u => !u.VoucherId.HasValue);
                }
            }

            var totalCount = await query.CountAsync();

        
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                var isDescending = parameters.SortDirection?.ToLower() == "desc";

                query = parameters.SortBy.ToLower() switch
                {
                    "id" => isDescending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
                    "nome" => isDescending ? query.OrderByDescending(u => u.Nome) : query.OrderBy(u => u.Nome),
                    "email" => isDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                    "tipo" => isDescending ? query.OrderByDescending(u => u.Tipo) : query.OrderBy(u => u.Tipo),
                    "equipeid" => isDescending ? query.OrderByDescending(u => u.EquipeId) : query.OrderBy(u => u.EquipeId),
                    _ => query.OrderBy(u => u.Id)
                };
            }

            
            var data = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Equipe)
                .Include(u => u.Voucher)
                .Include(u => u.Avaliacoes)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .Include(u => u.Equipe)
                .Include(u => u.Voucher)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            // Usar Count ao invés de AnyAsync para evitar problemas com Oracle
            var count = await _context.Usuarios.CountAsync(u => u.Email == email);
            return count > 0;
        }
    }
}
