using CampCorr.Context;
using CampCorr.Repositories.Interfaces;
using CampCorr.ViewModels;
using CampCorr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System.Threading.Tasks;
using MockQueryable.Moq;

namespace CampCorr.Areas.Piloto.Controllers
{
    [Area("Piloto")]
    
    public class PilotosController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly string nomeUsuario;
        private readonly IPilotoRepository _pilotoRepository;

        public PilotosController(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUsuarioRepository usuarioRepository, IPilotoRepository pilotoRepository)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _usuarioRepository = usuarioRepository;
            nomeUsuario = signInManager.Context.User.Identity.Name;
            _pilotoRepository = pilotoRepository;
        }
        [Authorize(Roles = "Piloto")]
        public IActionResult Cadastro()
        {
            var usuario = _context.Users.Where(x => x.UserName == _signInManager.Context.User.Identity.Name).FirstOrDefault();
            var piloto = _context.Pilotos.Where(x => x.UsuarioId == usuario.Id).FirstOrDefault();
            if (piloto != null)
            {
                PilotoViewModel pilotoVM = new PilotoViewModel()
                {
                    UserLogin = _signInManager.Context.User.Identity.Name,
                    PilotoId = piloto.PilotoId,
                    NomePiloto = piloto.Nome,
                    TipoSanguineo = piloto.TipoSanguineo,
                    Peso = piloto.Peso,
                    DataNascimento = (piloto.DataNascimento == null ? DateTime.Today.AddYears(-50) : piloto.DataNascimento),
                    DescricaoPiloto = piloto.Descricao,
                    Telefone = usuario.PhoneNumber,
                    Email = usuario.Email

                };

            return View(pilotoVM);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Piloto")]
        public async Task<IActionResult> Cadastro(string NomePiloto, string Email, string DescricaoPiloto, string Telefone,[Bind("PilotoId,DataNascimento,Peso,TipoSanguineo")] Models.Piloto piloto)
        {
            if (ModelState.IsValid)
            {
                var idUsuario = _usuarioRepository.BuscarIdUsuarioPorNome(nomeUsuario);
                piloto.Nome = NomePiloto; 
                piloto.Descricao = DescricaoPiloto;
                piloto.UsuarioId = idUsuario;
                var usuario = await _context.Users.Where(x => x.Id == idUsuario).FirstOrDefaultAsync();
                usuario.Email = Email; usuario.PhoneNumber = Telefone;
                _context.Update(piloto);
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            else
            {
                var erro = ModelState.Values;
            }
            return RedirectToAction("Index", "Home", new {area = ""});
        }
    }
}
