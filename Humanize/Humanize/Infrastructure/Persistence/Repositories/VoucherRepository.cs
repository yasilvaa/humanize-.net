using Microsoft.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence.Entities;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly HumanizeContext _context;

        public VoucherRepository(HumanizeContext context)
        {
            _context = context;
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
