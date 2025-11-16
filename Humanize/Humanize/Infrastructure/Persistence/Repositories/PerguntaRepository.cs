using Microsoft.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence.Entities;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public class PerguntaRepository : IPerguntaRepository
    {
        private readonly HumanizeContext _context;

        public PerguntaRepository(HumanizeContext context)
        {
            _context = context;
        }

        public async Task<Pergunta> AddAsync(Pergunta pergunta)
        {
            _context.Perguntas.Add(pergunta);
            await _context.SaveChangesAsync();
            return pergunta;
        }

        public async Task<Pergunta> GetByIdAsync(int id)
        {
            return await _context.Perguntas
                .Include(p => p.Respostas)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pergunta>> GetAllAsync()
        {
            return await _context.Perguntas
                .Include(p => p.Respostas)
                .ToListAsync();
        }

        public async Task UpdateAsync(Pergunta pergunta)
        {
            _context.Entry(pergunta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pergunta = await _context.Perguntas.FindAsync(id);
            if (pergunta != null)
            {
                _context.Perguntas.Remove(pergunta);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Pergunta>> GetPerguntasWithRespostasAsync()
        {
            // Usar Count ao invés de Any para evitar problemas com Oracle
            return await _context.Perguntas
                .Include(p => p.Respostas)
                .Where(p => p.Respostas.Count() > 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pergunta>> SearchByTituloAsync(string titulo)
        {
            return await _context.Perguntas
                .Include(p => p.Respostas)
                .Where(p => p.Titulo.Contains(titulo))
                .ToListAsync();
        }
    }
}
