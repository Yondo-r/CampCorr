using Microsoft.AspNetCore.Identity;

namespace CampCorr.Services.Interfaces
{
    public interface IUsuarioService
    {
        List<IdentityUser> ListarUsuarios();
        public string BuscarIdUsuario(string nomeUsuario);
        Task<IdentityUser> BuscarUsuarioAsync(string usuarioId);
    }
}
