using Microsoft.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Infrastructure.Persistence.Repositories
{
    public class AvaliacaoRepository : IAvaliacaoRepository
    {
        private readonly HumanizeContext _context;

        public AvaliacaoRepository(HumanizeContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Avaliacao> Data, int TotalCount)> SearchAsync(AvaliacaoSearchParametersDTO parameters)
        {
            var query = _context.Avaliacoes
                .Include(a => a.Usuario)
                .Include(a => a.Respostas)
                .ThenInclude(r => r.Pergunta)
                .AsQueryable();

            // Filtros
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(a => a.Usuario.Nome.Contains(parameters.SearchTerm) ||
                   a.Usuario.Email.Contains(parameters.SearchTerm));
            }

            if (parameters.UsuarioId.HasValue)
            {
                query = query.Where(a => a.UsuarioId == parameters.UsuarioId.Value);
            }

            if (parameters.DataInicio.HasValue)
            {
                query = query.Where(a => a.DataHora >= parameters.DataInicio.Value);
            }

            if (parameters.DataFim.HasValue)
            {
                query = query.Where(a => a.DataHora <= parameters.DataFim.Value);
            }

            if (parameters.MinRespostas.HasValue)
            {
                query = query.Where(a => a.Respostas.Count >= parameters.MinRespostas.Value);
            }

            if (parameters.MaxRespostas.HasValue)
            {
                query = query.Where(a => a.Respostas.Count <= parameters.MaxRespostas.Value);
            }

            var totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                var isDescending = parameters.SortDirection?.ToLower() == "desc";

                query = parameters.SortBy.ToLower() switch
                {
                    "id" => isDescending ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id),
                    "datahora" => isDescending ? query.OrderByDescending(a => a.DataHora) : query.OrderBy(a => a.DataHora),
                    "usuarioid" => isDescending ? query.OrderByDescending(a => a.UsuarioId) : query.OrderBy(a => a.UsuarioId),
                    "totalrespostas" => isDescending ? query.OrderByDescending(a => a.Respostas.Count) : query.OrderBy(a => a.Respostas.Count),
                    _ => query.OrderByDescending(a => a.DataHora)
                };
            }
            else
            {
                query = query.OrderByDescending(a => a.DataHora);
            }

            var data = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (data, totalCount);
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
