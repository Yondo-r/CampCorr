using CampCorr.Services.Interfaces;
using CampCorr.Context;
using Microsoft.AspNetCore.Identity;
using CampCorr.Repositories.Interfaces;

namespace CampCorr.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(AppDbContext context, IUsuarioRepository usuarioRepository)
        {
            _context = context;
            _usuarioRepository = usuarioRepository;
        }

        public List<IdentityUser> ListarUsuarios()
        {
            return _context.Users.ToList();
        }
        public string BuscarIdUsuario (string nomeUsuario)
        {
            return _usuarioRepository.BuscarIdUsuarioPorNome(nomeUsuario);
        }
        public async Task<IdentityUser> BuscarUsuarioAsync(string usuarioId)
        {
            return await _usuarioRepository.BuscarUsuarioAsync(usuarioId);
        }
    }
}
