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
using CampCorr.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace CampCorr.Areas.Campeonato.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Adm")]
    public class CampeonatosController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICampeonatoService _campeonatoService;
        private readonly IUsuarioService _usuarioService;
        private readonly ITemporadaService _temporadasService;
        private readonly IPilotoService _pilotoService;
        private readonly string nomeUsuario;
        private readonly int campeonatoId;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUtilitarioService _utilitarioService;
        public CampeonatosController(SignInManager<IdentityUser> signInManager, ICampeonatoService icampeonatoService, IUsuarioService usuarioService, ITemporadaService temporadasService, IPilotoService pilotoService, IWebHostEnvironment webHostEnvironment, IUtilitarioService utilitarioService)
        {
            _signInManager = signInManager;
            nomeUsuario = signInManager.Context.User.Identity.Name;
            _campeonatoService = icampeonatoService;
            _usuarioService = usuarioService;
            _temporadasService = temporadasService;
            _pilotoService = pilotoService;
            campeonatoId = _campeonatoService.BuscarIdCampeonato(nomeUsuario);
            _webHostEnvironment = webHostEnvironment;
            _utilitarioService = utilitarioService;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public IActionResult Create()
        //{
        //    ViewData["CampeonatoId"] = new SelectList(_context.Campeonatos, "CampeonatoId", "CampeonatoName");
        //    return View();
        //}

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
            var campeonatoId = _campeonatoService.BuscarIdCampeonato(nomeUsuario);
            var userId = _usuarioService.BuscarIdUsuario(nomeUsuario);


            var campeonato = await _campeonatoService.BuscarCampeonato(campeonatoId);
            if (campeonato == null)
            {
                return NotFound();
            }
            CampeonatoViewModel campeonatoVM = new CampeonatoViewModel(campeonato.CampeonatoId, campeonato.UserId, nomeUsuario, _utilitarioService.MontaImagem(campeonato.Logo));
            if (campeonato.Logo != null && campeonato.Logo.Length > 0)
            {
                ViewBag.Logo = campeonato.Logo;
            }
            campeonatoVM.Temporadas = _temporadasService.ListarTemporadasDoCampeonato(campeonatoId);

            return View(campeonatoVM);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile logo, [Bind("IdCampeonato,Logo,NomeCampeonato")] CampeonatoViewModel campeonatoVm)
        {
            Models.Campeonato campeonato = await _campeonatoService.BuscarCampeonato(campeonatoVm.IdCampeonato);
            if (logo != null && logo.Length > 0)
            {
                campeonato.Logo = _utilitarioService.PreparaImagem(logo);
            }
            campeonatoVm.NomeCampeonato = nomeUsuario;

            if (ModelState.IsValid)
            {
                _campeonatoService.Atualizar(campeonato);

                CampeonatoViewModel campeonatoVM = new CampeonatoViewModel(campeonato.CampeonatoId, campeonato.UserId, nomeUsuario, _utilitarioService.MontaImagem(campeonato.Logo));
                campeonatoVM.Temporadas = _temporadasService.ListarTemporadasDoCampeonato(campeonato.CampeonatoId);
                if (campeonato.Logo != null && campeonato.Logo.Length > 0)
                {
                    ViewBag.Logo = campeonato.Logo;
                }
                return View(campeonatoVM);
            }
            return View(campeonatoVm);
        }

        public async Task<IActionResult> BuscarPilotos(PilotoViewModel filtro, int pageindex = 1, string sort = "UserLogin")
        {
            List<PilotoViewModel> listaPilotoVm = MontarListaPilotoVmCampeonato();


            var resultado = listaPilotoVm.AsQueryable();

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
                pilotoCampeonato.CampeonatoId = _campeonatoService.BuscarIdCampeonato(nomeUsuario);
                _pilotoService.SalvarPilotoCampeonato(pilotoCampeonato);
            }

            var listaPilotoVm = MontarListaPilotoVmCampeonato();
            var resultado = listaPilotoVm.AsQueryable();
            resultado = resultado.BuildMockDbSet().Object;
            var model = await PagingList.CreateAsync(resultado, 5, 1, "UserLogin", "UserLogin");
            return RedirectToAction("BuscarPilotos", model);
        }
        public ActionResult VisualizarPilotosCampeonato()
        {
            var pilotosCampeonato = _pilotoService.ListarPilotosVmCampeonato(campeonatoId);

            return View(pilotosCampeonato);
        }
        public ActionResult RemovePilotoCampeonato(int pilotoId)
        {
            var idCampeonato = campeonatoId;
            _pilotoService.RemovePilotoCampeonato(pilotoId, idCampeonato);


            var listaPilotosCampeonato = _pilotoService.ListarPilotosVmCampeonato(idCampeonato);
            return View("VisualizarPilotosCampeonato", listaPilotosCampeonato);
        }

        private List<PilotoViewModel> MontarListaPilotoVmCampeonato()
        {
            List<PilotoViewModel> listaPilotoVm = new List<PilotoViewModel>();
            var listaUsuario = _usuarioService.ListarUsuarios();
            var listaPilotos = _pilotoService.ListarPilotos();
            var listaPilotosCadastrados = _pilotoService.ListarPilotosCampeonato();
            foreach (var item in listaUsuario)
            {
                var piloto = listaPilotos.Where(x => x.UsuarioId == item.Id).FirstOrDefault();
                if (piloto != null)
                {
                    var pilotoCadastrado = listaPilotosCadastrados
                        .Where(x => x.PilotoId == piloto.PilotoId && x.CampeonatoId == campeonatoId)
                        .FirstOrDefault();
                    if (pilotoCadastrado == null)
                    {
                        listaPilotoVm.AddRange(new[]
                        {
                            new PilotoViewModel(piloto.PilotoId, item.UserName, piloto.Nome)
                        });
                    }
                }
            }
            return listaPilotoVm;
        }
    }
}
