using Microsoft.AspNetCore.Mvc;
using Humanize.Infrastructure.Persistence.Repositories;
using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly ILogger<VoucherController> _logger;

        public VoucherController(IVoucherRepository voucherRepository, ILogger<VoucherController> logger)
        {
            _voucherRepository = voucherRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoucherDTO>>> GetAll()
        {
            try
            {
                var vouchers = await _voucherRepository.GetAllAsync();
                var vouchersDto = vouchers.Select(v => new VoucherDTO
                {
                    Id = v.Id,
                    Nome = v.Nome,
                    Loja = v.Loja,
                    Validade = v.Validade,
                    Status = v.Status,
                    TotalUsuarios = v.Usuarios?.Count ?? 0
                });

                return Ok(vouchersDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os vouchers");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VoucherDTO>> GetById(int id)
        {
            try
            {
                var voucher = await _voucherRepository.GetByIdAsync(id);
                if (voucher == null)
                {
                    return NotFound($"Voucher com ID {id} não encontrado");
                }

                var voucherDto = new VoucherDTO
                {
                    Id = voucher.Id,
                    Nome = voucher.Nome,
                    Loja = voucher.Loja,
                    Validade = voucher.Validade,
                    Status = voucher.Status,
                    TotalUsuarios = voucher.Usuarios?.Count ?? 0
                };

                return Ok(voucherDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar voucher com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("validos")]
        public async Task<ActionResult<IEnumerable<VoucherDTO>>> GetVouchersValidos()
        {
            try
            {
                var vouchers = await _voucherRepository.GetVouchersValidosAsync();
                var vouchersDto = vouchers.Select(v => new VoucherDTO
                {
                    Id = v.Id,
                    Nome = v.Nome,
                    Loja = v.Loja,
                    Validade = v.Validade,
                    Status = v.Status,
                    TotalUsuarios = v.Usuarios?.Count ?? 0
                });

                return Ok(vouchersDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar vouchers válidos");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("vencidos")]
        public async Task<ActionResult<IEnumerable<VoucherDTO>>> GetVouchersVencidos()
        {
            try
            {
                var vouchers = await _voucherRepository.GetVouchersVencidosAsync();
                var vouchersDto = vouchers.Select(v => new VoucherDTO
                {
                    Id = v.Id,
                    Nome = v.Nome,
                    Loja = v.Loja,
                    Validade = v.Validade,
                    Status = v.Status,
                    TotalUsuarios = v.Usuarios?.Count ?? 0
                });

                return Ok(vouchersDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar vouchers vencidos");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<VoucherDTO>>> GetByStatus(string status)
        {
            try
            {
                var vouchers = await _voucherRepository.GetVouchersByStatusAsync(status);
                var vouchersDto = vouchers.Select(v => new VoucherDTO
                {
                    Id = v.Id,
                    Nome = v.Nome,
                    Loja = v.Loja,
                    Validade = v.Validade,
                    Status = v.Status,
                    TotalUsuarios = v.Usuarios?.Count ?? 0
                });

                return Ok(vouchersDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar vouchers com status {Status}", status);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPost]
        public async Task<ActionResult<VoucherDTO>> Create([FromBody] CreateVoucherDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var voucher = new Voucher
                {
                    Nome = createDto.Nome,
                    Loja = createDto.Loja,
                    Validade = createDto.Validade,
                    Status = createDto.Status
                };

                var voucherCriado = await _voucherRepository.AddAsync(voucher);

                var voucherDto = new VoucherDTO
                {
                    Id = voucherCriado.Id,
                    Nome = voucherCriado.Nome,
                    Loja = voucherCriado.Loja,
                    Validade = voucherCriado.Validade,
                    Status = voucherCriado.Status,
                    TotalUsuarios = 0
                };

                return CreatedAtAction(nameof(GetById), new { id = voucherCriado.Id }, voucherDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar voucher");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateVoucherDTO updateDto)
        {
            try
            {
                var voucher = await _voucherRepository.GetByIdAsync(id);
                if (voucher == null)
                {
                    return NotFound($"Voucher com ID {id} não encontrado");
                }

                if (!string.IsNullOrEmpty(updateDto.Nome))
                    voucher.Nome = updateDto.Nome;

                if (!string.IsNullOrEmpty(updateDto.Loja))
                    voucher.Loja = updateDto.Loja;

                if (updateDto.Validade.HasValue)
                    voucher.Validade = updateDto.Validade.Value;

                if (!string.IsNullOrEmpty(updateDto.Status))
                    voucher.Status = updateDto.Status;

                await _voucherRepository.UpdateAsync(voucher);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar voucher com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var voucher = await _voucherRepository.GetByIdAsync(id);
                if (voucher == null)
                {
                    return NotFound($"Voucher com ID {id} não encontrado");
                }

                if (voucher.Usuarios != null && voucher.Usuarios.Count > 0)
                {
                    return BadRequest("Não é possível excluir um voucher que possui usuários");
                }

                await _voucherRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar voucher com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
