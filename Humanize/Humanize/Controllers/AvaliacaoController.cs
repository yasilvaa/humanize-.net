using Humanize.DTOs;
using Humanize.Infrastructure.Persistence.Entities;
using Humanize.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Humanize.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly ILogger<AvaliacaoController> _logger;

        public AvaliacaoController(IAvaliacaoRepository avaliacaoRepository, ILogger<AvaliacaoController> logger)
        {
            _avaliacaoRepository = avaliacaoRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AvaliacaoDTO>>> GetAll()
        {
            try
            {
                var avaliacoes = await _avaliacaoRepository.GetAllAsync();
                var avaliacoesDto = avaliacoes.Select(a => new AvaliacaoDTO
                {
                    Id = a.Id,
                    DataHora = a.DataHora,
                    UsuarioId = a.UsuarioId,
                    UsuarioNome = a.Usuario?.Nome,
                    TotalRespostas = a.Respostas?.Count ?? 0
                });

                return Ok(avaliacoesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as avaliações");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AvaliacaoDTO>> GetById(int id)
        {
            try
            {
                var avaliacao = await _avaliacaoRepository.GetByIdAsync(id);
                if (avaliacao == null)
                {
                    return NotFound($"Avaliação com ID {id} não encontrada");
                }

                var avaliacaoDto = new AvaliacaoDTO
                {
                    Id = avaliacao.Id,
                    DataHora = avaliacao.DataHora,
                    UsuarioId = avaliacao.UsuarioId,
                    UsuarioNome = avaliacao.Usuario?.Nome,
                    TotalRespostas = avaliacao.Respostas?.Count ?? 0
                };

                return Ok(avaliacaoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar avaliação com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<AvaliacaoDTO>>> GetByUsuarioId(int usuarioId)
        {
            try
            {
                var avaliacoes = await _avaliacaoRepository.GetByUsuarioIdAsync(usuarioId);
                var avaliacoesDto = avaliacoes.Select(a => new AvaliacaoDTO
                {
                    Id = a.Id,
                    DataHora = a.DataHora,
                    UsuarioId = a.UsuarioId,
                    UsuarioNome = a.Usuario?.Nome,
                    TotalRespostas = a.Respostas?.Count ?? 0
                });

                return Ok(avaliacoesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar avaliações do usuário {UsuarioId}", usuarioId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("periodo")]
        public async Task<ActionResult<IEnumerable<AvaliacaoDTO>>> GetByPeriodo([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
        {
            try
            {
                var avaliacoes = await _avaliacaoRepository.GetByPeriodoAsync(dataInicio, dataFim);
                var avaliacoesDto = avaliacoes.Select(a => new AvaliacaoDTO
                {
                    Id = a.Id,
                    DataHora = a.DataHora,
                    UsuarioId = a.UsuarioId,
                    UsuarioNome = a.Usuario?.Nome,
                    TotalRespostas = a.Respostas?.Count ?? 0
                });

                return Ok(avaliacoesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar avaliações por período");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("usuario/{usuarioId}/ultima")]
        public async Task<ActionResult<AvaliacaoDTO>> GetUltimaAvaliacaoUsuario(int usuarioId)
        {
            try
            {
                var avaliacao = await _avaliacaoRepository.GetUltimaAvaliacaoUsuarioAsync(usuarioId);
                if (avaliacao == null)
                {
                    return NotFound($"Nenhuma avaliação encontrada para o usuário {usuarioId}");
                }

                var avaliacaoDto = new AvaliacaoDTO
                {
                    Id = avaliacao.Id,
                    DataHora = avaliacao.DataHora,
                    UsuarioId = avaliacao.UsuarioId,
                    UsuarioNome = avaliacao.Usuario?.Nome,
                    TotalRespostas = avaliacao.Respostas?.Count ?? 0
                };

                return Ok(avaliacaoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar última avaliação do usuário {UsuarioId}", usuarioId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPost]
        public async Task<ActionResult<AvaliacaoDTO>> Create([FromBody] CreateAvaliacaoDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var avaliacao = new Avaliacao
                {
                    UsuarioId = createDto.UsuarioId,
                    DataHora = createDto.DataHora ?? DateTime.Now
                };

                var avaliacaoCriada = await _avaliacaoRepository.AddAsync(avaliacao);

                var avaliacaoDto = new AvaliacaoDTO
                {
                    Id = avaliacaoCriada.Id,
                    DataHora = avaliacaoCriada.DataHora,
                    UsuarioId = avaliacaoCriada.UsuarioId,
                    TotalRespostas = 0
                };

                return CreatedAtAction(nameof(GetById), new { id = avaliacaoCriada.Id }, avaliacaoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar avaliação");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreateAvaliacaoDTO updateDto)
        {
            try
            {
                var avaliacao = await _avaliacaoRepository.GetByIdAsync(id);
                if (avaliacao == null)
                {
                    return NotFound($"Avaliação com ID {id} não encontrada");
                }

                if (updateDto.DataHora.HasValue)
                    avaliacao.DataHora = updateDto.DataHora.Value;

                await _avaliacaoRepository.UpdateAsync(avaliacao);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar avaliação com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var avaliacao = await _avaliacaoRepository.GetByIdAsync(id);
                if (avaliacao == null)
                {
                    return NotFound($"Avaliação com ID {id} não encontrada");
                }

                // Verifica se a avaliação tem respostas
                if (avaliacao.Respostas != null && avaliacao.Respostas.Count > 0)
                {
                    return BadRequest("Não é possível excluir uma avaliação que possui respostas");
                }

                await _avaliacaoRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar avaliação com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
