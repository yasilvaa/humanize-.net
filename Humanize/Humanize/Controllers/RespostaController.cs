using Microsoft.AspNetCore.Mvc;
using Humanize.Infrastructure.Persistence.Repositories;
using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RespostaController : ControllerBase
    {
        private readonly IRespostaRepository _respostaRepository;
        private readonly ILogger<RespostaController> _logger;

        public RespostaController(IRespostaRepository respostaRepository, ILogger<RespostaController> logger)
        {
            _respostaRepository = respostaRepository;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<RespostaDTO>>> GetAll()
        {
            try
            {
                var respostas = await _respostaRepository.GetAllAsync();
                var respostasDto = respostas.Select(r => new RespostaDTO
                {
                    Id = r.Id,
                    Humor = r.Humor,
                    Categoria = r.Categoria,
                    Comentario = r.Comentario,
                    AvaliacaoId = r.AvaliacaoId,
                    AvaliacaoDataHora = r.Avaliacao?.DataHora,
                    PerguntaId = r.PerguntaId,
                    PerguntaTitulo = r.Pergunta?.Titulo
                });

                return Ok(respostasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as respostas");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaDTO>> GetById(int id)
        {
            try
            {
                var resposta = await _respostaRepository.GetByIdAsync(id);
                if (resposta == null)
                {
                    return NotFound($"Resposta com ID {id} não encontrada");
                }

                var respostaDto = new RespostaDTO
                {
                    Id = resposta.Id,
                    Humor = resposta.Humor,
                    Categoria = resposta.Categoria,
                    Comentario = resposta.Comentario,
                    AvaliacaoId = resposta.AvaliacaoId,
                    AvaliacaoDataHora = resposta.Avaliacao?.DataHora,
                    PerguntaId = resposta.PerguntaId,
                    PerguntaTitulo = resposta.Pergunta?.Titulo
                };

                return Ok(respostaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar resposta com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }


        [HttpGet("avaliacao/{avaliacaoId}")]
        public async Task<ActionResult<IEnumerable<RespostaDTO>>> GetByAvaliacaoId(int avaliacaoId)
        {
            try
            {
                var respostas = await _respostaRepository.GetByAvaliacaoIdAsync(avaliacaoId);
                var respostasDto = respostas.Select(r => new RespostaDTO
                {
                    Id = r.Id,
                    Humor = r.Humor,
                    Categoria = r.Categoria,
                    Comentario = r.Comentario,
                    AvaliacaoId = r.AvaliacaoId,
                    AvaliacaoDataHora = r.Avaliacao?.DataHora,
                    PerguntaId = r.PerguntaId,
                    PerguntaTitulo = r.Pergunta?.Titulo
                });

                return Ok(respostasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar respostas da avaliação {AvaliacaoId}", avaliacaoId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }


        [HttpGet("pergunta/{perguntaId}")]
        public async Task<ActionResult<IEnumerable<RespostaDTO>>> GetByPerguntaId(int perguntaId)
        {
            try
            {
                var respostas = await _respostaRepository.GetByPerguntaIdAsync(perguntaId);
                var respostasDto = respostas.Select(r => new RespostaDTO
                {
                    Id = r.Id,
                    Humor = r.Humor,
                    Categoria = r.Categoria,
                    Comentario = r.Comentario,
                    AvaliacaoId = r.AvaliacaoId,
                    AvaliacaoDataHora = r.Avaliacao?.DataHora,
                    PerguntaId = r.PerguntaId,
                    PerguntaTitulo = r.Pergunta?.Titulo
                });

                return Ok(respostasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar respostas da pergunta {PerguntaId}", perguntaId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }


        [HttpGet("humor")]
        public async Task<ActionResult<IEnumerable<RespostaDTO>>> GetByHumorRange([FromQuery] int humorMin, [FromQuery] int humorMax)
        {
            try
            {
                var respostas = await _respostaRepository.GetByHumorRangeAsync(humorMin, humorMax);
                var respostasDto = respostas.Select(r => new RespostaDTO
                {
                    Id = r.Id,
                    Humor = r.Humor,
                    Categoria = r.Categoria,
                    Comentario = r.Comentario,
                    AvaliacaoId = r.AvaliacaoId,
                    AvaliacaoDataHora = r.Avaliacao?.DataHora,
                    PerguntaId = r.PerguntaId,
                    PerguntaTitulo = r.Pergunta?.Titulo
                });

                return Ok(respostasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar respostas por faixa de humor");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("categoria/{categoria}")]
        public async Task<ActionResult<IEnumerable<RespostaDTO>>> GetByCategoria(int categoria)
        {
            try
            {
                var respostas = await _respostaRepository.GetByCategoriaAsync(categoria);
                var respostasDto = respostas.Select(r => new RespostaDTO
                {
                    Id = r.Id,
                    Humor = r.Humor,
                    Categoria = r.Categoria,
                    Comentario = r.Comentario,
                    AvaliacaoId = r.AvaliacaoId,
                    AvaliacaoDataHora = r.Avaliacao?.DataHora,
                    PerguntaId = r.PerguntaId,
                    PerguntaTitulo = r.Pergunta?.Titulo
                });

                return Ok(respostasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar respostas da categoria {Categoria}", categoria);
                return StatusCode(500, "Erro interno do servidor");
            }
        }


        [HttpGet("com-comentario")]
        public async Task<ActionResult<IEnumerable<RespostaDTO>>> GetRespostasWithComentario()
        {
            try
            {
                var respostas = await _respostaRepository.GetRespostasWithComentarioAsync();
                var respostasDto = respostas.Select(r => new RespostaDTO
                {
                    Id = r.Id,
                    Humor = r.Humor,
                    Categoria = r.Categoria,
                    Comentario = r.Comentario,
                    AvaliacaoId = r.AvaliacaoId,
                    AvaliacaoDataHora = r.Avaliacao?.DataHora,
                    PerguntaId = r.PerguntaId,
                    PerguntaTitulo = r.Pergunta?.Titulo
                });

                return Ok(respostasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar respostas com comentário");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("pergunta/{perguntaId}/media-humor")]
        public async Task<ActionResult<double>> GetMediaHumorByPergunta(int perguntaId)
        {
            try
            {
                var media = await _respostaRepository.GetMediaHumorByPerguntaAsync(perguntaId);
                return Ok(new { PerguntaId = perguntaId, MediaHumor = media });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular média de humor da pergunta {PerguntaId}", perguntaId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }


        [HttpPost]
        public async Task<ActionResult<RespostaDTO>> Create([FromBody] CreateRespostaDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var resposta = new Resposta
                {
                    Humor = createDto.Humor,
                    Categoria = createDto.Categoria,
                    Comentario = createDto.Comentario,
                    AvaliacaoId = createDto.AvaliacaoId,
                    PerguntaId = createDto.PerguntaId
                };

                var respostaCriada = await _respostaRepository.AddAsync(resposta);

                var respostaDto = new RespostaDTO
                {
                    Id = respostaCriada.Id,
                    Humor = respostaCriada.Humor,
                    Categoria = respostaCriada.Categoria,
                    Comentario = respostaCriada.Comentario,
                    AvaliacaoId = respostaCriada.AvaliacaoId,
                    PerguntaId = respostaCriada.PerguntaId
                };

                return CreatedAtAction(nameof(GetById), new { id = respostaCriada.Id }, respostaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar resposta");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreateRespostaDTO updateDto)
        {
            try
            {
                var resposta = await _respostaRepository.GetByIdAsync(id);
                if (resposta == null)
                {
                    return NotFound($"Resposta com ID {id} não encontrada");
                }

                resposta.Humor = updateDto.Humor;
                resposta.Categoria = updateDto.Categoria;
                resposta.Comentario = updateDto.Comentario;

                await _respostaRepository.UpdateAsync(resposta);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar resposta com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var resposta = await _respostaRepository.GetByIdAsync(id);
                if (resposta == null)
                {
                    return NotFound($"Resposta com ID {id} não encontrada");
                }

                await _respostaRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar resposta com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
