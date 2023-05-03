using CampCorr.Context;
using Microsoft.AspNetCore.Identity;
using CampCorr.Repositories.Interfaces;

namespace CampCorr.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;

        public UsuarioRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        
        public UsuarioRepository() { }
        
        
        public string BuscarIdUsuarioPorNome(string nomeUsuario)
        {
            return _context.Users.Where(x => x.UserName == nomeUsuario).FirstOrDefault().Id;
        }
    }
}
