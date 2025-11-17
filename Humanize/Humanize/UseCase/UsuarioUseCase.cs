using Humanize.Infrastructure.Persistence.Entities;
using Humanize.Infrastructure.Persistence.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace Humanize.UseCase;

public class UsuarioUseCase : IUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Usuario> RegisterAsync(string nome, string email, string senha)
    {
        if (await _usuarioRepository.EmailExistsAsync(email))
            throw new InvalidOperationException("Email já está em uso");

        var usuario = new Usuario
        {
            Nome = nome,
            Email = email,
            Senha = HashSenha(senha),
            Tipo = "Usuario", 
            EquipeId = 1
        };

        return await _usuarioRepository.AddAsync(usuario);
    }

    public async Task<Usuario> LoginAsync(string email, string senha)
    {
        var senhaHash = HashSenha(senha);
        var usuario = await _usuarioRepository.GetByEmailAsync(email);

        if (usuario == null || usuario.Senha != senhaHash)
            throw new InvalidOperationException("Email ou senha inválidos");

        return usuario;
    }

    public Task<bool> LogoutAsync()
    {
        return Task.FromResult(true);
    }

    public async Task<Usuario> GetByIdAsync(int id)
    {
        return await _usuarioRepository.GetByIdAsync(id);
    }

    public async Task<List<Usuario>> GetAllAsync()
    {
        var parameters = new DTOs.UsuarioSearchParametersDTO
        {
            Page = 1,
            PageSize = 1000
        };

        var (data, totalCount) = await _usuarioRepository.SearchAsync(parameters);
        return data.ToList();
    }

    public async Task DeleteAsync(int id)
    {
        await _usuarioRepository.DeleteAsync(id);
    }

    private string HashSenha(string senha)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        return Convert.ToBase64String(bytes);
    }
}