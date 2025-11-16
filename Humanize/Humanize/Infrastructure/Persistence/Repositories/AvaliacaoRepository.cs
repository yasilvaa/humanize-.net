using Microsoft.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence.Entities;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public class AvaliacaoRepository : IAvaliacaoRepository
    {
        private readonly HumanizeContext _context;

        public AvaliacaoRepository(HumanizeContext context)
        {
            _context = context;
        }

        public async Task<Avaliacao> AddAsync(Avaliacao avaliacao)
        {
            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();
            return avaliacao;
        }

        public async Task<Avaliacao> GetByIdAsync(int id)
        {
            return await _context.Avaliacoes
                .Include(a => a.Usuario)
                .Include(a => a.Respostas)
                .ThenInclude(r => r.Pergunta)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Avaliacao>> GetAllAsync()
        {
            return await _context.Avaliacoes
                .Include(a => a.Usuario)
                .Include(a => a.Respostas)
                .ToListAsync();
        }

        public async Task<IEnumerable<Avaliacao>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Avaliacoes
                .Include(a => a.Usuario)
                .Include(a => a.Respostas)
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.DataHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Avaliacao>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.Avaliacoes
                .Include(a => a.Usuario)
                .Include(a => a.Respostas)
                .Where(a => a.DataHora >= dataInicio && a.DataHora <= dataFim)
                .OrderByDescending(a => a.DataHora)
                .ToListAsync();
        }

        public async Task UpdateAsync(Avaliacao avaliacao)
        {
            _context.Entry(avaliacao).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(id);
            if (avaliacao != null)
            {
                _context.Avaliacoes.Remove(avaliacao);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Avaliacao>> GetAvaliacoesWithRespostasAsync()
        {
            return await _context.Avaliacoes
                .Include(a => a.Usuario)
                .Include(a => a.Respostas)
                .ThenInclude(r => r.Pergunta)
                .Where(a => a.Respostas.Any())
                .ToListAsync();
        }

        public async Task<Avaliacao> GetUltimaAvaliacaoUsuarioAsync(int usuarioId)
        {
            return await _context.Avaliacoes
                .Include(a => a.Usuario)
                .Include(a => a.Respostas)
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.DataHora)
                .FirstOrDefaultAsync();
        }
    }
}
