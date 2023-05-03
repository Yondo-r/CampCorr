using Microsoft.AspNetCore.Mvc;
using CampCorr.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CampCorr.Models;
using ReflectionIT.Mvc.Paging;
using CampCorr.ViewModels;
using Microsoft.AspNetCore.Identity;
using CampCorr.Repositories;
using CampCorr.Repositories.Interfaces;
using MockQueryable.Moq;

namespace CampCorr.Areas.Campeonato.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Adm")]
    public class CampeonatosController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICampeonatoRepository _campeonatoRepository;
        private readonly string nomeUsuario;

        public CampeonatosController(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUsuarioRepository usuarioRepository, ICampeonatoRepository campeonatoRepository)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _usuarioRepository = usuarioRepository;
            _campeonatoRepository = campeonatoRepository;
            nomeUsuario = signInManager.Context.User.Identity.Name;
        }

        //public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Nome")
        //{
        //    var resultado = _context.Users.AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(filter))
        //    {
        //        resultado = resultado.Where(p => p.UserName.Contains(filter));
        //        //Exibir usuários que sejam apenas campeonatos

        //    }
        //    var model = await PagingList.CreateAsync(resultado, 5, pageindex, sort, "Nome");
        //    model.RouteValue = new RouteValueDictionary { { "filter", filter } };

        //    return View(model);
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewData["CampeonatoId"] = new SelectList(_context.Campeonatos, "CampeonatoId", "CampeonatoName");
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create([Bind("CampeonatoId,NomeCampeonato,Logo,Senha")] LoginCampeonatoViewModel campeonatoVm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //         Usar repositório para gravar no banco
        //        _campeonatoRepository.Salvar(campeonatoVm);
        //        return RedirectToAction("Create", "Temporadas", new { campeonatoVm/*.CampeonatoId */});
        //    }
        //    return View(campeonatoVm);
        //}

        public async Task<IActionResult> Edit()
        {
            //UsuarioRepository usuarios = 
            var campeonatoId = _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(_signInManager.Context.User.Identity.Name);
            var userId = _usuarioRepository.BuscarIdUsuarioPorNome(_signInManager.Context.User.Identity.Name);
            if (_context.Campeonatos == null)
            {
                return NotFound();
            }

            var campeonato = await _context.Campeonatos.FindAsync(campeonatoId);
            if (campeonato == null)
            {
                return NotFound();
            }
            CampeonatoViewModel campeonatoVM = new CampeonatoViewModel(campeonatoId, userId, _signInManager.Context.User.Identity.Name, campeonato.Logo);


            campeonatoVM.Temporadas = _context.Temporadas.Where(x => x.CampeonatoId == campeonatoId).ToList();

            return View(campeonatoVM);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CampeonatoId,Nome,Logo")] Models.Campeonato campeonato)
        {
            campeonato.UserId = _usuarioRepository.BuscarIdUsuarioPorNome(_signInManager.Context.User.Identity.Name);
            campeonato.CampeonatoId = _context.Campeonatos.AsNoTracking().Where(x => x.UserId == campeonato.UserId).FirstOrDefault().CampeonatoId;//camp.CampeonatoId;
            //if (campeonatoId != campeonato.CampeonatoId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campeonato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!CampeonatoExists(campeonato.CampeonatoId))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                CampeonatoViewModel campeonatoVM = new CampeonatoViewModel(campeonato.CampeonatoId, campeonato.UserId, _signInManager.Context.User.Identity.Name, campeonato.Logo);
                return View(campeonatoVM);
                //return RedirectToAction(nameof(Index));
            }
            return View(campeonato);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Campeonatos == null)
            {
                return Problem("Entity set 'AppDbContext.Categorias'  is null.");
            }
            var campeonatos = await _context.Campeonatos.FindAsync(id);
            if (campeonatos != null)
            {
                _context.Campeonatos.Remove(campeonatos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Campeonatos == null)
            {
                return Problem("Entity set 'AppDbContext.Categorias'  is null.");
            }
            var campeonatos = await _context.Campeonatos.FindAsync(id);
            if (campeonatos != null)
            {
                _context.Campeonatos.Remove(campeonatos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginCampeonatoViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        public async Task<IActionResult> BuscarPilotos(PilotoViewModel filtro, int pageindex = 1, string sort = "UserLogin")
        {
            var listaUsuario = _context.Users.ToList();
            var listaPilotos = _context.Pilotos.ToList();
            var resultadoVm = new List<PilotoViewModel>();
            var listaPilotosCadastrados = _context.PilotosCampeonatos.ToList();
            foreach (var item in listaUsuario)
            {
                var piloto = listaPilotos.Where(x => x.UsuarioId == item.Id).FirstOrDefault();
                if (piloto != null)
                {
                    var pilotoCadastrado = listaPilotosCadastrados.Where(x => x.PilotoId == piloto.PilotoId).FirstOrDefault();
                    if (pilotoCadastrado == null)
                    {
                        resultadoVm.AddRange(new[]
                        {
                            new PilotoViewModel(piloto.PilotoId, item.UserName, piloto.Nome)
                        });
                    }
                }
            }
            //var pilotosNaoAdicionados = PreencherListaPilotosNaoAdicionado(resultadoVm);

            var resultado = resultadoVm.AsQueryable();

            string nomePiloto = String.IsNullOrEmpty(filtro.NomePiloto) ? "" : filtro.NomePiloto;
            string userLogin = String.IsNullOrEmpty(filtro.UserLogin) ? "" : filtro.UserLogin;

            if (!string.IsNullOrEmpty(filtro.UserLogin) || !string.IsNullOrEmpty(filtro.NomePiloto))
                resultado = resultado.Where(p => p.UserLogin.Contains(userLogin) && p.NomePiloto.Contains(nomePiloto));

            resultado = resultado.BuildMockDbSet().Object;

            var model = await PagingList.CreateAsync(resultado, 5, pageindex, sort, "UserLogin");
            model.RouteValue = new RouteValueDictionary { { "filtro.UserLogin", filtro.UserLogin } };
            return View(model);
        }

        public async Task<IActionResult> AddPilotoCampeonato(int pilotoId, [Bind("Id, PilotoId,CampeonatoId")] PilotoCampeonato pilotoCampeonato)
        {

            if (ModelState.IsValid)
            {
                pilotoCampeonato.PilotoId = pilotoId;
                pilotoCampeonato.CampeonatoId = _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(nomeUsuario);
                _context.Add(pilotoCampeonato);
                await _context.SaveChangesAsync();
            }
            var listaUsuario = _context.Users.ToList();
            var listaPilotos = _context.Pilotos.ToList();
            var listaPilotosCadastrados = _context.PilotosCampeonatos.ToList();
            var resultadoVm = new List<PilotoViewModel>();
            foreach (var item in listaUsuario)
            {
                var piloto = listaPilotos.Where(x => x.UsuarioId == item.Id).FirstOrDefault();
                if (piloto != null)
                {
                    var pilotoCadastrado = listaPilotosCadastrados.Where(x => x.PilotoId == piloto.PilotoId).FirstOrDefault();
                    if (pilotoCadastrado == null)
                    {
                        resultadoVm.AddRange(new[]
                        {
                            new PilotoViewModel(piloto.PilotoId, item.UserName, piloto.Nome)
                        });
                    }
                }
            }

            var resultado = resultadoVm.AsQueryable();
            resultado = resultado.BuildMockDbSet().Object;
            var model = await PagingList.CreateAsync(resultado, 5, 1, "UserLogin", "UserLogin");
            return RedirectToAction("BuscarPilotos",model);
        }
        public ActionResult VisualizarPilotosCampeonato()
        {
            var pilotosCampeonato = PreencherPilotosAdicionados(_campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(nomeUsuario));

            return View(pilotosCampeonato);
        }
        public ActionResult RemovePilotoCampeonato(int pilotoId)
        {
            var idCampeonato = _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(nomeUsuario);
            _campeonatoRepository.RemovePilotoCampeonato(pilotoId, idCampeonato);


            var listaPilotosCampeonato = PreencherPilotosAdicionados(idCampeonato);
            return View("VisualizarPilotosCampeonato", listaPilotosCampeonato);
        }




        private bool CampeonatoExists(int id)
        {
            return _context.Campeonatos.Any(e => e.CampeonatoId == id);
        }
        private List<PilotoViewModel> PreencherListaPilotosNaoAdicionado(List<PilotoViewModel> pilotosCadastrados)
        {
            List<PilotoViewModel> pilotosNaoCadastrados = new List<PilotoViewModel>();
            foreach (var piloto in pilotosCadastrados)
            {
                if (!pilotosCadastrados.Contains(piloto))
                {
                    pilotosNaoCadastrados.Add(piloto);
                }
            }
            return pilotosNaoCadastrados;
        }

        private List<PilotoViewModel> PreencherPilotosAdicionados(int idCampeonato)
        {
            List<PilotoViewModel> pilotosCampeonato = new List<PilotoViewModel>();
            var idsPilotosCampeonato = _context.Pilotos.Join(_context.PilotosCampeonatos,
                p => p.PilotoId,
                pc => pc.PilotoId, (p, pc) => new { p, pc })
                .Join(_context.Campeonatos,
                x => x.pc.CampeonatoId,
                c => c.CampeonatoId, (p1, c) => new { p1, c })
                .Where(y => y.c.CampeonatoId == idCampeonato).Select(x => x.p1.p.UsuarioId).ToList();
            foreach (var userId in idsPilotosCampeonato)
            {
                PilotoViewModel pilotoVm = new PilotoViewModel() { };
                var piloto = _context.Pilotos.Where(x => x.UsuarioId == userId).FirstOrDefault();
                pilotoVm.NomePiloto = piloto.Nome;
                pilotoVm.PilotoId = piloto.PilotoId;
                pilotoVm.UserLogin = _context.Users.Where(x => x.Id == userId).Select(x => x.UserName).FirstOrDefault();
                pilotosCampeonato.Add(pilotoVm);
            }
            return pilotosCampeonato;
        }
        private CampeonatoViewModel PreencheDadosCampeonato(Models.Campeonato camp, CampeonatoViewModel campeonato)
        {
            camp.Logo = campeonato.Logo;
            camp.CampeonatoId = campeonato.IdCampeonato;
            camp.UserId = campeonato.UserId;

            return campeonato;
        }
    }

}
