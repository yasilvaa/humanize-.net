using Humanize.DTOs;
using Humanize.Infrastructure.Persistence.Entities;
using Humanize.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Humanize.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerguntaController : ControllerBase
    {
        private readonly IPerguntaRepository _perguntaRepository;
        private readonly ILogger<PerguntaController> _logger;

        public PerguntaController(IPerguntaRepository perguntaRepository, ILogger<PerguntaController> logger)
        {
            _perguntaRepository = perguntaRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerguntaDTO>>> GetAll()
        {
            try
            {
                var perguntas = await _perguntaRepository.GetAllAsync();
                var perguntasDto = perguntas.Select(p => new PerguntaDTO
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    TotalRespostas = p.Respostas?.Count ?? 0
                });

                return Ok(perguntasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as perguntas");
                return StatusCode(500, "Erro interno do servidor");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PerguntaDTO>> GetById(int id)
        {
            try
            {
                var pergunta = await _perguntaRepository.GetByIdAsync(id);
                if (pergunta == null)
                {
                    return NotFound($"Pergunta com ID {id} não encontrada");
                }

                var perguntaDto = new PerguntaDTO
                {
                    Id = pergunta.Id,
                    Titulo = pergunta.Titulo,
                    TotalRespostas = pergunta.Respostas?.Count ?? 0
                };

                return Ok(perguntaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pergunta com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("buscar/{titulo}")]
        public async Task<ActionResult<IEnumerable<PerguntaDTO>>> SearchByTitulo(string titulo)
        {
            try
            {
                var perguntas = await _perguntaRepository.SearchByTituloAsync(titulo);
                var perguntasDto = perguntas.Select(p => new PerguntaDTO
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    TotalRespostas = p.Respostas?.Count ?? 0
                });

                return Ok(perguntasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar perguntas com título {Titulo}", titulo);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("com-respostas")]
        public async Task<ActionResult<IEnumerable<PerguntaDTO>>> GetPerguntasWithRespostas()
        {
            try
            {
                var perguntas = await _perguntaRepository.GetPerguntasWithRespostasAsync();
                var perguntasDto = perguntas.Select(p => new PerguntaDTO
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    TotalRespostas = p.Respostas?.Count ?? 0
                });

                return Ok(perguntasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar perguntas com respostas");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PerguntaDTO>> Create([FromBody] CreatePerguntaDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var pergunta = new Pergunta
                {
                    Titulo = createDto.Titulo
                };

                var perguntaCriada = await _perguntaRepository.AddAsync(pergunta);

                var perguntaDto = new PerguntaDTO
                {
                    Id = perguntaCriada.Id,
                    Titulo = perguntaCriada.Titulo,
                    TotalRespostas = 0
                };

                return CreatedAtAction(nameof(GetById), new { id = perguntaCriada.Id }, perguntaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pergunta");
                return StatusCode(500, "Erro interno do servidor");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreatePerguntaDTO updateDto)
        {
            try
            {
                var pergunta = await _perguntaRepository.GetByIdAsync(id);
                if (pergunta == null)
                {
                    return NotFound($"Pergunta com ID {id} não encontrada");
                }

                if (!string.IsNullOrEmpty(updateDto.Titulo))
                    pergunta.Titulo = updateDto.Titulo;

                await _perguntaRepository.UpdateAsync(pergunta);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar pergunta com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var pergunta = await _perguntaRepository.GetByIdAsync(id);
                if (pergunta == null)
                {
                    return NotFound($"Pergunta com ID {id} não encontrada");
                }

                // Verifica se a pergunta tem respostas
                if (pergunta.Respostas != null && pergunta.Respostas.Count > 0)
                {
                    return BadRequest("Não é possível excluir uma pergunta que possui respostas");
                }

                await _perguntaRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar pergunta com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
