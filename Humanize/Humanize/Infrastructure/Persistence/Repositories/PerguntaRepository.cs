using Microsoft.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public class PerguntaRepository : IPerguntaRepository
    {
        private readonly HumanizeContext _context;

        public PerguntaRepository(HumanizeContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Pergunta> Data, int TotalCount)> SearchAsync(PerguntaSearchParametersDTO parameters)
        {
            var query = _context.Perguntas
                .Include(p => p.Respostas)
                .AsQueryable();

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(p => p.Titulo.Contains(parameters.SearchTerm));
            }

            if (!string.IsNullOrEmpty(parameters.Titulo))
            {
                query = query.Where(p => p.Titulo.Contains(parameters.Titulo));
            }

            if (parameters.MinRespostas.HasValue)
            {
                query = query.Where(p => p.Respostas.Count >= parameters.MinRespostas.Value);
            }

            if (parameters.MaxRespostas.HasValue)
            {
                query = query.Where(p => p.Respostas.Count <= parameters.MaxRespostas.Value);
            }

            if (parameters.HasRespostas.HasValue)
            {
                if (parameters.HasRespostas.Value)
                {
                    query = query.Where(p => p.Respostas.Count > 0);
                }
                else
                {
                    query = query.Where(p => p.Respostas.Count == 0);
                }
            }

            var totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                var isDescending = parameters.SortDirection?.ToLower() == "desc";

                query = parameters.SortBy.ToLower() switch
                {
                    "id" => isDescending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
                    "titulo" => isDescending ? query.OrderByDescending(p => p.Titulo) : query.OrderBy(p => p.Titulo),
                    "totalrespostas" => isDescending ? query.OrderByDescending(p => p.Respostas.Count) : query.OrderBy(p => p.Respostas.Count),
                    _ => query.OrderBy(p => p.Id)
                };
            }

            var data = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (data, totalCount);
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
