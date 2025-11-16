using Humanize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
      private readonly HumanizeContext _context;

        public UsuarioRepository(HumanizeContext context)
   {
      _context = context;
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
