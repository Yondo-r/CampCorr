using Microsoft.AspNetCore.Identity;

namespace CampCorr.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        string BuscarIdUsuarioPorNome(string nomeUsuario);
        Task<IdentityUser> BuscarUsuarioAsync(string usuarioId);
        
    }
}
