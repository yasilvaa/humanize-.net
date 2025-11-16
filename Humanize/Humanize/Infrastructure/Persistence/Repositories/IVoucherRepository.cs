using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public interface IVoucherRepository
    {
        Task<Voucher> AddAsync(Voucher voucher);
        Task<Voucher> GetByIdAsync(int id);
        Task<IEnumerable<Voucher>> GetAllAsync();
        Task<IEnumerable<Voucher>> GetVouchersValidosAsync();
        Task<IEnumerable<Voucher>> GetVouchersVencidosAsync();
        Task<IEnumerable<Voucher>> GetVouchersByStatusAsync(string status);
        Task UpdateAsync(Voucher voucher);
        Task DeleteAsync(int id);
        Task<IEnumerable<Voucher>> GetVouchersWithUsuariosAsync();
        Task<(IEnumerable<Voucher> Data, int TotalCount)> SearchAsync(VoucherSearchParametersDTO parameters);
    }
}
