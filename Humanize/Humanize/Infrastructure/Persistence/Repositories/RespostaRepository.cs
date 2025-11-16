using Microsoft.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public class RespostaRepository : IRespostaRepository
    {
        private readonly HumanizeContext _context;

        public RespostaRepository(HumanizeContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Resposta> Data, int TotalCount)> SearchAsync(RespostaSearchParametersDTO parameters)
        {
            var query = _context.Respostas
                .Include(r => r.Avaliacao)
                    .ThenInclude(a => a.Usuario)
                .Include(r => r.Pergunta)
                .AsQueryable();

            // Filtros
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(r => r.Pergunta.Titulo.Contains(parameters.SearchTerm) ||
                                          r.Avaliacao.Usuario.Nome.Contains(parameters.SearchTerm) ||
                                          (r.Comentario != null && r.Comentario.Contains(parameters.SearchTerm)));
            }

            if (parameters.AvaliacaoId.HasValue)
            {
                query = query.Where(r => r.AvaliacaoId == parameters.AvaliacaoId.Value);
            }

            if (parameters.PerguntaId.HasValue)
            {
                query = query.Where(r => r.PerguntaId == parameters.PerguntaId.Value);
            }

            if (parameters.MinHumor.HasValue)
            {
                query = query.Where(r => r.Humor >= parameters.MinHumor.Value);
            }

            if (parameters.MaxHumor.HasValue)
            {
                query = query.Where(r => r.Humor <= parameters.MaxHumor.Value);
            }

            if (parameters.Categoria.HasValue)
            {
                query = query.Where(r => r.Categoria == parameters.Categoria.Value);
            }

            if (parameters.HasComentario.HasValue)
            {
                if (parameters.HasComentario.Value)
                {
                    query = query.Where(r => !string.IsNullOrEmpty(r.Comentario));
                }
                else
                {
                    query = query.Where(r => string.IsNullOrEmpty(r.Comentario));
                }
            }

            if (parameters.DataInicio.HasValue)
            {
                query = query.Where(r => r.Avaliacao.DataHora >= parameters.DataInicio.Value);
            }

            if (parameters.DataFim.HasValue)
            {
                query = query.Where(r => r.Avaliacao.DataHora <= parameters.DataFim.Value);
            }

            var totalCount = await query.CountAsync();

            
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                var isDescending = parameters.SortDirection?.ToLower() == "desc";

                query = parameters.SortBy.ToLower() switch
                {
                    "id" => isDescending ? query.OrderByDescending(r => r.Id) : query.OrderBy(r => r.Id),
                    "humor" => isDescending ? query.OrderByDescending(r => r.Humor) : query.OrderBy(r => r.Humor),
                    "categoria" => isDescending ? query.OrderByDescending(r => r.Categoria) : query.OrderBy(r => r.Categoria),
                    "avaliacaoid" => isDescending ? query.OrderByDescending(r => r.AvaliacaoId) : query.OrderBy(r => r.AvaliacaoId),
                    "perguntaid" => isDescending ? query.OrderByDescending(r => r.PerguntaId) : query.OrderBy(r => r.PerguntaId),
                    "datahora" => isDescending ? query.OrderByDescending(r => r.Avaliacao.DataHora) : query.OrderBy(r => r.Avaliacao.DataHora),
                    _ => query.OrderBy(r => r.Id)
                };
            }

            var data = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (data, totalCount);
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
