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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioRepository usuarioRepository, ILogger<UsuarioController> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<ActionResult<PaginatedResultDTO<UsuarioDTO>>> Search([FromQuery] UsuarioSearchParametersDTO parameters)
        {
            try
            {
                var (data, totalCount) = await _usuarioRepository.SearchAsync(parameters);

                var usuariosDto = data.Select(u => new UsuarioDTO
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Tipo = u.Tipo,
                    EquipeId = u.EquipeId,
                    EquipeNome = u.Equipe?.Nome,
                    VoucherId = u.VoucherId,
                    VoucherNome = u.Voucher?.Nome
                }).ToList();

                var result = new PaginatedResultDTO<UsuarioDTO>
                {
                    Data = usuariosDto,
                    TotalCount = totalCount,
                    Page = parameters.Page,
                    PageSize = parameters.PageSize
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuários");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetById(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound($"Usuário com ID {id} não encontrado");
                }

                var usuarioDto = new UsuarioDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Tipo = usuario.Tipo,
                    EquipeId = usuario.EquipeId,
                    EquipeNome = usuario.Equipe?.Nome,
                    VoucherId = usuario.VoucherId,
                    VoucherNome = usuario.Voucher?.Nome
                };

                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuário com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }


        [HttpGet("email/{email}")]
        public async Task<ActionResult<UsuarioDTO>> GetByEmail(string email)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByEmailAsync(email);
                if (usuario == null)
                {
                    return NotFound($"Usuário com email {email} não encontrado");
                }

                var usuarioDto = new UsuarioDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Tipo = usuario.Tipo,
                    EquipeId = usuario.EquipeId,
                    EquipeNome = usuario.Equipe?.Nome,
                    VoucherId = usuario.VoucherId,
                    VoucherNome = usuario.Voucher?.Nome
                };

                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuário com email {Email}", email);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> Create([FromBody] CreateUsuarioDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verifica se email já existe
                if (await _usuarioRepository.EmailExistsAsync(createDto.Email))
                {
                    return Conflict($"Email {createDto.Email} já está em uso");
                }

                var usuario = new Usuario
                {
                    Nome = createDto.Nome,
                    Email = createDto.Email,
                    Senha = createDto.Senha, 
                    Tipo = createDto.Tipo,
                    EquipeId = createDto.EquipeId,
                    VoucherId = createDto.VoucherId
                };

                var usuarioCriado = await _usuarioRepository.AddAsync(usuario);

                var usuarioDto = new UsuarioDTO
                {
                    Id = usuarioCriado.Id,
                    Nome = usuarioCriado.Nome,
                    Email = usuarioCriado.Email,
                    Tipo = usuarioCriado.Tipo,
                    EquipeId = usuarioCriado.EquipeId,
                    VoucherId = usuarioCriado.VoucherId
                };

                return CreatedAtAction(nameof(GetById), new { id = usuarioCriado.Id }, usuarioDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar usuário");
                return StatusCode(500, "Erro interno do servidor");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateUsuarioDTO updateDto)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound($"Usuário com ID {id} não encontrado");
                }

                if (!string.IsNullOrEmpty(updateDto.Nome))
                    usuario.Nome = updateDto.Nome;

                if (!string.IsNullOrEmpty(updateDto.Email))
                {
                    if (await _usuarioRepository.EmailExistsAsync(updateDto.Email) && usuario.Email != updateDto.Email)
                    {
                        return Conflict($"Email {updateDto.Email} já está em uso");
                    }
                    usuario.Email = updateDto.Email;
                }

                if (!string.IsNullOrEmpty(updateDto.Senha))
                    usuario.Senha = updateDto.Senha;

                if (!string.IsNullOrEmpty(updateDto.Tipo))
                    usuario.Tipo = updateDto.Tipo;

                if (updateDto.EquipeId.HasValue)
                    usuario.EquipeId = updateDto.EquipeId.Value;

                if (updateDto.VoucherId.HasValue)
                    usuario.VoucherId = updateDto.VoucherId;
                else if (updateDto.VoucherId == null && updateDto.GetType().GetProperty("VoucherId") != null)
                    usuario.VoucherId = null;

                await _usuarioRepository.UpdateAsync(usuario);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound($"Usuário com ID {id} não encontrado");
                }

                await _usuarioRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar usuário com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("check-email/{email}")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            try
            {
                var exists = await _usuarioRepository.EmailExistsAsync(email);
                return Ok(new { EmailExists = exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar email {Email}", email);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
