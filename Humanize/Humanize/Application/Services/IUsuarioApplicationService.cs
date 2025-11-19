using Humanize.DTOs;

namespace Humanize.Application.Services
{
    public interface IUsuarioApplicationService
    {
        Task<UsuarioDTO> CriarUsuarioAsync(CreateUsuarioDTO createDto);
        Task<UsuarioDTO?> ObterUsuarioPorIdAsync(int id);
        Task<UsuarioDTO?> ObterUsuarioPorEmailAsync(string email);
        Task<PaginatedResultDTO<UsuarioDTO>> BuscarUsuariosAsync(UsuarioSearchParametersDTO parameters);
        Task AtualizarUsuarioAsync(int id, UpdateUsuarioDTO updateDto);
        Task ExcluirUsuarioAsync(int id);
        Task<bool> EmailExisteAsync(string email);
    }
}