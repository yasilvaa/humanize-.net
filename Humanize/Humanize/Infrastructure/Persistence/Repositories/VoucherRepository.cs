using Microsoft.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly HumanizeContext _context;

        public VoucherRepository(HumanizeContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Voucher> Data, int TotalCount)> SearchAsync(VoucherSearchParametersDTO parameters)
        {
            var query = _context.Vouchers
                .Include(v => v.Usuarios)
                .AsQueryable();

            // Filtros
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(v => v.Nome.Contains(parameters.SearchTerm) ||
                                         v.Loja.Contains(parameters.SearchTerm) ||
                                         (v.Status != null && v.Status.Contains(parameters.SearchTerm)));
            }

            if (!string.IsNullOrEmpty(parameters.Nome))
            {
                query = query.Where(v => v.Nome.Contains(parameters.Nome));
            }

            if (!string.IsNullOrEmpty(parameters.Loja))
            {
                query = query.Where(v => v.Loja.Contains(parameters.Loja));
            }

            if (!string.IsNullOrEmpty(parameters.Status))
            {
                query = query.Where(v => v.Status == parameters.Status);
            }

            if (parameters.ValidadeInicio.HasValue)
            {
                query = query.Where(v => v.Validade >= parameters.ValidadeInicio.Value);
            }

            if (parameters.ValidadeFim.HasValue)
            {
                query = query.Where(v => v.Validade <= parameters.ValidadeFim.Value);
            }

            if (parameters.IsVencido.HasValue)
            {
                if (parameters.IsVencido.Value)
                {
                    query = query.Where(v => v.Validade.HasValue && v.Validade.Value < DateTime.Now);
                }
                else
                {
                    query = query.Where(v => !v.Validade.HasValue || v.Validade.Value >= DateTime.Now);
                }
            }

            if (parameters.MinUsuarios.HasValue)
            {
                query = query.Where(v => v.Usuarios.Count >= parameters.MinUsuarios.Value);
            }

            if (parameters.MaxUsuarios.HasValue)
            {
                query = query.Where(v => v.Usuarios.Count <= parameters.MaxUsuarios.Value);
            }

            var totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                var isDescending = parameters.SortDirection?.ToLower() == "desc";

                query = parameters.SortBy.ToLower() switch
                {
                    "id" => isDescending ? query.OrderByDescending(v => v.Id) : query.OrderBy(v => v.Id),
                    "nome" => isDescending ? query.OrderByDescending(v => v.Nome) : query.OrderBy(v => v.Nome),
                    "loja" => isDescending ? query.OrderByDescending(v => v.Loja) : query.OrderBy(v => v.Loja),
                    "status" => isDescending ? query.OrderByDescending(v => v.Status) : query.OrderBy(v => v.Status),
                    "validade" => isDescending ? query.OrderByDescending(v => v.Validade) : query.OrderBy(v => v.Validade),
                    "totalusuarios" => isDescending ? query.OrderByDescending(v => v.Usuarios.Count) : query.OrderBy(v => v.Usuarios.Count),
                    _ => query.OrderBy(v => v.Id)
                };
            }

    
            var data = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        public async Task<Voucher> AddAsync(Voucher voucher)
        {
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }

        public async Task<Voucher> GetByIdAsync(int id)
        {
            return await _context.Vouchers
                .Include(v => v.Usuarios)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Voucher>> GetAllAsync()
        {
            return await _context.Vouchers
                .Include(v => v.Usuarios)
                .ToListAsync();
        }

        public async Task<IEnumerable<Voucher>> GetVouchersValidosAsync()
        {
            return await _context.Vouchers
                .Include(v => v.Usuarios)
                .Where(v => v.Validade > DateTime.Now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Voucher>> GetVouchersVencidosAsync()
        {
            return await _context.Vouchers
                .Include(v => v.Usuarios)
                .Where(v => v.Validade <= DateTime.Now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Voucher>> GetVouchersByStatusAsync(string status)
        {
            return await _context.Vouchers
                .Include(v => v.Usuarios)
                .Where(v => v.Status == status)
                .ToListAsync();
        }

        public async Task UpdateAsync(Voucher voucher)
        {
            _context.Entry(voucher).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher != null)
            {
                _context.Vouchers.Remove(voucher);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Voucher>> GetVouchersWithUsuariosAsync()
        {
            return await _context.Vouchers
                .Include(v => v.Usuarios)
                .Where(v => v.Usuarios.Any())
                .ToListAsync();
        }
    }
}
