using Microsoft.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence.Entities;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public class RespostaRepository : IRespostaRepository
    {
        private readonly HumanizeContext _context;

        public RespostaRepository(HumanizeContext context)
        {
            _context = context;
        }

        public async Task<Resposta> AddAsync(Resposta resposta)
        {
            _context.Respostas.Add(resposta);
            await _context.SaveChangesAsync();
            return resposta;
        }

        public async Task<Resposta> GetByIdAsync(int id)
        {
            return await _context.Respostas
                .Include(r => r.Avaliacao)
                    .ThenInclude(a => a.Usuario)
                .Include(r => r.Pergunta)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Resposta>> GetAllAsync()
        {
            return await _context.Respostas
                .Include(r => r.Avaliacao)
                    .ThenInclude(a => a.Usuario)
                .Include(r => r.Pergunta)
                .ToListAsync();
        }

        public async Task<IEnumerable<Resposta>> GetByAvaliacaoIdAsync(int avaliacaoId)
        {
            return await _context.Respostas
                .Include(r => r.Avaliacao)
                .Include(r => r.Pergunta)
                .Where(r => r.AvaliacaoId == avaliacaoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Resposta>> GetByPerguntaIdAsync(int perguntaId)
        {
            return await _context.Respostas
                .Include(r => r.Avaliacao)
                    .ThenInclude(a => a.Usuario)
                .Include(r => r.Pergunta)
                .Where(r => r.PerguntaId == perguntaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Resposta>> GetByHumorRangeAsync(int humorMin, int humorMax)
        {
            return await _context.Respostas
                .Include(r => r.Avaliacao)
                    .ThenInclude(a => a.Usuario)
                .Include(r => r.Pergunta)
                .Where(r => r.Humor >= humorMin && r.Humor <= humorMax)
                .ToListAsync();
        }

        public async Task<IEnumerable<Resposta>> GetByCategoriaAsync(int categoria)
        {
            return await _context.Respostas
                .Include(r => r.Avaliacao)
                    .ThenInclude(a => a.Usuario)
                .Include(r => r.Pergunta)
                .Where(r => r.Categoria == categoria)
                .ToListAsync();
        }

        public async Task UpdateAsync(Resposta resposta)
        {
            _context.Entry(resposta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var resposta = await _context.Respostas.FindAsync(id);
            if (resposta != null)
            {
                _context.Respostas.Remove(resposta);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<double> GetMediaHumorByPerguntaAsync(int perguntaId)
        {
            var respostas = await _context.Respostas
                .Where(r => r.PerguntaId == perguntaId)
                .Select(r => r.Humor)
                .ToListAsync();

            // Usar Count ao invés de Any para evitar problemas com Oracle
            return respostas.Count > 0 ? respostas.Average() : 0;
        }

        public async Task<IEnumerable<Resposta>> GetRespostasWithComentarioAsync()
        {
            return await _context.Respostas
                .Include(r => r.Avaliacao)
                    .ThenInclude(a => a.Usuario)
                .Include(r => r.Pergunta)
                .Where(r => !string.IsNullOrEmpty(r.Comentario))
                .ToListAsync();
        }
    }
}
