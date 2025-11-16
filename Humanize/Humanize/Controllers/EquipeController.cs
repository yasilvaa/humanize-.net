using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence;
using Humanize.Infrastructure.Persistence.Repositories;
using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipeController : ControllerBase
    {
        private readonly IEquipeRepository _equipeRepository;
        private readonly ILogger<EquipeController> _logger;

        public EquipeController(IEquipeRepository equipeRepository, ILogger<EquipeController> logger)
        {
            _equipeRepository = equipeRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipeDTO>>> GetAll()
        {
            try
            {
                var equipes = await _equipeRepository.GetAllAsync();

                var equipesDto = equipes?.Select(e => new EquipeDTO
                {
                    Id = e.Id,
                    Nome = e.Nome ?? string.Empty,
                    TotalUsuarios = e.Usuarios?.Count ?? 0
                }).ToList() ?? new List<EquipeDTO>();

                return Ok(equipesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as equipes");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EquipeDTO>> GetById(int id)
        {
            try
            {
                var equipe = await _equipeRepository.GetByIdAsync(id);
                if (equipe == null)
                {
                    return NotFound($"Equipe com ID {id} não encontrada");
                }

                var equipeDto = new EquipeDTO
                {
                    Id = equipe.Id,
                    Nome = equipe.Nome ?? string.Empty,
                    TotalUsuarios = equipe.Usuarios?.Count ?? 0
                };

                return Ok(equipeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar equipe com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<EquipeDTO>> GetByNome(string nome)
        {
            try
            {
                var equipe = await _equipeRepository.GetByNomeAsync(nome);
                if (equipe == null)
                {
                    return NotFound($"Equipe com nome {nome} não encontrada");
                }

                var equipeDto = new EquipeDTO
                {
                    Id = equipe.Id,
                    Nome = equipe.Nome ?? string.Empty,
                    TotalUsuarios = equipe.Usuarios?.Count ?? 0
                };

                return Ok(equipeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar equipe com nome {Nome}", nome);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("com-usuarios")]
        public async Task<ActionResult<IEnumerable<EquipeDTO>>> GetEquipesWithUsuarios()
        {
            try
            {
                var equipes = await _equipeRepository.GetEquipesWithUsuariosAsync();

                var equipesDto = equipes?.Select(e => new EquipeDTO
                {
                    Id = e.Id,
                    Nome = e.Nome ?? string.Empty,
                    TotalUsuarios = e.Usuarios?.Count ?? 0
                }).ToList() ?? new List<EquipeDTO>();

                return Ok(equipesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar equipes com usuários");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPost]
        public async Task<ActionResult<EquipeDTO>> Create([FromBody] CreateEquipeDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _equipeRepository.NomeExistsAsync(createDto.Nome))
                {
                    return Conflict($"Nome {createDto.Nome} já está em uso");
                }

                var equipe = new Equipe
                {
                    Nome = createDto.Nome
                };

                var equipeCruida = await _equipeRepository.AddAsync(equipe);

                var equipeDto = new EquipeDTO
                {
                    Id = equipeCruida.Id,
                    Nome = equipeCruida.Nome,
                    TotalUsuarios = 0
                };

                return CreatedAtAction(nameof(GetById), new { id = equipeCruida.Id }, equipeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar equipe");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateEquipeDTO updateDto)
        {
            try
            {
                var equipe = await _equipeRepository.GetByIdAsync(id);
                if (equipe == null)
                {
                    return NotFound($"Equipe com ID {id} não encontrada");
                }

                if (!string.IsNullOrEmpty(updateDto.Nome))
                {
                    if (await _equipeRepository.NomeExistsAsync(updateDto.Nome) && equipe.Nome != updateDto.Nome)
                    {
                        return Conflict($"Nome {updateDto.Nome} já está em uso");
                    }
                    equipe.Nome = updateDto.Nome;
                }

                await _equipeRepository.UpdateAsync(equipe);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar equipe com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var equipe = await _equipeRepository.GetByIdAsync(id);
                if (equipe == null)
                {
                    return NotFound($"Equipe com ID {id} não encontrada");
                }

                if (equipe.Usuarios != null && equipe.Usuarios.Count > 0)
                {
                    return BadRequest("Não é possível excluir uma equipe que possui usuários");
                }

                await _equipeRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar equipe com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
