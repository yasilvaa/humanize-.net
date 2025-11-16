using Microsoft.AspNetCore.Mvc;
using Humanize.Infrastructure.Persistence.Repositories;
using Humanize.Infrastructure.Persistence.Entities;
using Humanize.DTOs;

namespace Humanize.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HateoasController : ControllerBase
    {
   private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEquipeRepository _equipeRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IPerguntaRepository _perguntaRepository;
        private readonly ILogger<HateoasController> _logger;

        public HateoasController(
     IUsuarioRepository usuarioRepository,
          IEquipeRepository equipeRepository,
            IVoucherRepository voucherRepository,
     IPerguntaRepository perguntaRepository,
    ILogger<HateoasController> logger)
        {
        _usuarioRepository = usuarioRepository;
            _equipeRepository = equipeRepository;
            _voucherRepository = voucherRepository;
       _perguntaRepository = perguntaRepository;
    _logger = logger;
        }

        /// <summary>
        /// GET: Obter usuário por ID com links HATEOAS
        /// </summary>
   [HttpGet("usuarios/{id}")]
        public async Task<ActionResult<UsuarioHateoasDTO>> GetUsuario(int id)
        {
        try
{
     var usuario = await _usuarioRepository.GetByIdAsync(id);
    if (usuario == null)
     {
    return NotFound($"Usuário com ID {id} não encontrado");
    }

                var usuarioHateoas = new UsuarioHateoasDTO
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

// Adicionar links HATEOAS
     usuarioHateoas.Links.AddRange(new List<HateoasLinkDTO>
      {
         new() { Href = $"/api/hateoas/usuarios/{id}", Rel = "self", Method = "GET" },
         new() { Href = $"/api/hateoas/usuarios/{id}", Rel = "update", Method = "PUT" },
            new() { Href = $"/api/hateoas/usuarios/{id}", Rel = "delete", Method = "DELETE" },
            new() { Href = "/api/hateoas/usuarios", Rel = "create", Method = "POST" },
         new() { Href = $"/api/hateoas/equipes/{usuario.EquipeId}", Rel = "equipe", Method = "GET" }
    });

     if (usuario.VoucherId.HasValue)
    {
                    usuarioHateoas.Links.Add(new HateoasLinkDTO 
     { 
  Href = $"/api/hateoas/vouchers/{usuario.VoucherId}", 
         Rel = "voucher", 
             Method = "GET" 
      });
 }

             return Ok(usuarioHateoas);
            }
     catch (Exception ex)
      {
  _logger.LogError(ex, "Erro ao buscar usuário com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// POST: Criar nova equipe com links HATEOAS
/// </summary>
        [HttpPost("equipes")]
public async Task<ActionResult<EquipeHateoasDTO>> CreateEquipe([FromBody] CreateEquipeDTO createDto)
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

  var equipeCriada = await _equipeRepository.AddAsync(equipe);

     var equipeHateoas = new EquipeHateoasDTO
  {
      Id = equipeCriada.Id,
             Nome = equipeCriada.Nome,
           TotalUsuarios = 0
       };

       // Adicionar links HATEOAS
      equipeHateoas.Links.AddRange(new List<HateoasLinkDTO>
                {
      new() { Href = $"/api/hateoas/equipes/{equipeCriada.Id}", Rel = "self", Method = "GET" },
 new() { Href = $"/api/hateoas/equipes/{equipeCriada.Id}", Rel = "update", Method = "PUT" },
      new() { Href = $"/api/hateoas/equipes/{equipeCriada.Id}", Rel = "delete", Method = "DELETE" },
 new() { Href = "/api/hateoas/equipes", Rel = "create", Method = "POST" },
 new() { Href = "/api/equipe", Rel = "all-equipes", Method = "GET" },
  new() { Href = "/api/usuario", Rel = "usuarios", Method = "GET" }
    });

                return CreatedAtAction(nameof(GetUsuario), new { id = equipeCriada.Id }, equipeHateoas);
   }
     catch (Exception ex)
            {
           _logger.LogError(ex, "Erro ao criar equipe");
      return StatusCode(500, "Erro interno do servidor");
        }
        }

        /// <summary>
   /// PUT: Atualizar voucher com links HATEOAS
        /// </summary>
  [HttpPut("vouchers/{id}")]
        public async Task<ActionResult<VoucherHateoasDTO>> UpdateVoucher(int id, [FromBody] UpdateVoucherDTO updateDto)
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

        // Buscar voucher atualizado com relacionamentos
                var voucherAtualizado = await _voucherRepository.GetByIdAsync(id);

      var voucherHateoas = new VoucherHateoasDTO
{
                 Id = voucherAtualizado.Id,
         Nome = voucherAtualizado.Nome,
        Loja = voucherAtualizado.Loja,
        Validade = voucherAtualizado.Validade,
           Status = voucherAtualizado.Status,
    TotalUsuarios = voucherAtualizado.Usuarios?.Count ?? 0
          };

         // Adicionar links HATEOAS
    voucherHateoas.Links.AddRange(new List<HateoasLinkDTO>
          {
           new() { Href = $"/api/hateoas/vouchers/{id}", Rel = "self", Method = "GET" },
         new() { Href = $"/api/hateoas/vouchers/{id}", Rel = "update", Method = "PUT" },
  new() { Href = $"/api/hateoas/vouchers/{id}", Rel = "delete", Method = "DELETE" },
         new() { Href = "/api/hateoas/vouchers", Rel = "create", Method = "POST" },
          new() { Href = "/api/voucher", Rel = "all-vouchers", Method = "GET" },
 new() { Href = "/api/voucher/validos", Rel = "vouchers-validos", Method = "GET" }
     });

        return Ok(voucherHateoas);
            }
            catch (Exception ex)
     {
           _logger.LogError(ex, "Erro ao atualizar voucher com ID {Id}", id);
    return StatusCode(500, "Erro interno do servidor");
    }
        }

  /// <summary>
        /// DELETE: Excluir pergunta com links HATEOAS de navegação
        /// </summary>
     [HttpDelete("perguntas/{id}")]
    public async Task<ActionResult<object>> DeletePergunta(int id)
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

      // Retornar links HATEOAS de navegação após exclusão
  var navigationLinks = new
 {
         Message = $"Pergunta com ID {id} foi excluída com sucesso",
 Links = new List<HateoasLinkDTO>
           {
   new() { Href = "/api/pergunta", Rel = "all-perguntas", Method = "GET" },
          new() { Href = "/api/hateoas/perguntas", Rel = "create", Method = "POST" },
          new() { Href = "/api/pergunta/com-respostas", Rel = "perguntas-com-respostas", Method = "GET" },
          new() { Href = "/api/resposta", Rel = "all-respostas", Method = "GET" }
             }
         };

    return Ok(navigationLinks);
            }
        catch (Exception ex)
            {
          _logger.LogError(ex, "Erro ao deletar pergunta com ID {Id}", id);
         return StatusCode(500, "Erro interno do servidor");
   }
        }

        /// <summary>
        /// GET: Endpoint para descoberta de recursos HATEOAS
        /// </summary>
        [HttpGet("discovery")]
        public ActionResult<object> GetApiDiscovery()
      {
    var discovery = new
          {
    ApiName = "Humanize HATEOAS API",
    Version = "1.0.0",
      Description = "API com implementação HATEOAS para sistema Humanize",
         AvailableResources = new
            {
     Usuarios = new List<HateoasLinkDTO>
           {
     new() { Href = "/api/hateoas/usuarios/{id}", Rel = "get-usuario", Method = "GET" },
        new() { Href = "/api/usuario", Rel = "all-usuarios", Method = "GET" },
       new() { Href = "/api/usuario", Rel = "create-usuario", Method = "POST" }
  },
   Equipes = new List<HateoasLinkDTO>
            {
 new() { Href = "/api/hateoas/equipes", Rel = "create-equipe", Method = "POST" },
    new() { Href = "/api/equipe", Rel = "all-equipes", Method = "GET" },
              new() { Href = "/api/equipe/{id}", Rel = "get-equipe", Method = "GET" }
},
           Vouchers = new List<HateoasLinkDTO>
        {
   new() { Href = "/api/hateoas/vouchers/{id}", Rel = "update-voucher", Method = "PUT" },
       new() { Href = "/api/voucher", Rel = "all-vouchers", Method = "GET" },
       new() { Href = "/api/voucher/{id}", Rel = "get-voucher", Method = "GET" }
              },
    Perguntas = new List<HateoasLinkDTO>
               {
     new() { Href = "/api/hateoas/perguntas/{id}", Rel = "delete-pergunta", Method = "DELETE" },
        new() { Href = "/api/pergunta", Rel = "all-perguntas", Method = "GET" },
  new() { Href = "/api/pergunta/{id}", Rel = "get-pergunta", Method = "GET" }
        }
    }
            };

            return Ok(discovery);
        }
    }
}