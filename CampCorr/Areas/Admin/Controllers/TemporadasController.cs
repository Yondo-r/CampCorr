using Microsoft.AspNetCore.Mvc;
using CampCorr.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using CampCorr.ViewModels;
using CampCorr.Services.Interfaces;

namespace CampCorr.Areas.Campeonato.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Adm")]
    public class TemporadasController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICampeonatoService _campeonatoService;
        private readonly ITemporadaService _temporadaService;
        private readonly IEtapaService _etapaService;
        private readonly ICircuitoService _circuitoService;
        private readonly IRegulamentoService _regulamentoService;
        private readonly IPilotoService _pilotoService;
        private readonly IUsuarioService _usuarioService;
        private readonly string nomeUsuario;
        private readonly int campeonatoId;

        public TemporadasController(SignInManager<IdentityUser> signInManager, ICampeonatoService campeonatoService, ITemporadaService temporadasService, IEtapaService etapaService, ICircuitoService circuitoService, IRegulamentoService regulamentoService, IPilotoService pilotoService, IUsuarioService usuarioService)
        {
            _signInManager = signInManager;
            nomeUsuario = signInManager.Context.User.Identity.Name;
            _campeonatoService = campeonatoService;
            campeonatoId = _campeonatoService.BuscarIdCampeonato(nomeUsuario);
            _temporadaService = temporadasService;
            _etapaService = etapaService;
            _circuitoService = circuitoService;
            _regulamentoService = regulamentoService;
            _pilotoService = pilotoService;
            _usuarioService = usuarioService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Regulamentos = _regulamentoService.ListarRegulamentos().ToList();
            TempData["campeonatoId"] = campeonatoId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("TemporadaId,QuantidadeEtapas,RegulamentoId")] Temporada temporada)
        {
            temporada.CampeonatoId = campeonatoId;
            if (ModelState.IsValid)
            {
                _temporadaService.Salvar(temporada);
                return RedirectToAction("Edit", new { ano = temporada.AnoTemporada });
            }
            return View(temporada);
        }

        public async Task<IActionResult> Edit(int ano)
        {
            ViewBag.Regulamentos = _regulamentoService.ListarRegulamentos();
            var temporada = await _temporadaService.BuscarTemporadaAsync(campeonatoId, ano);
            if (temporada == null)
            {
                return NotFound();
            }

            temporada.Etapas = _etapaService.ListarEtapasTemporada(temporada.TemporadaId);
            foreach (var item in temporada.Etapas)
            {
                item.Circuito = await _circuitoService.BuscarCircuitoAsync(item.CircuitoId);
            }
            CampeonatoViewModel campeonatoTemporadaVm = new CampeonatoViewModel()
            {
                Etapas = temporada.Etapas.OrderBy(x => x.Data).ToList(),
                AnoTemporada = temporada.AnoTemporada,
                QuantidadeEtapas = temporada.QuantidadeEtapas,
                Regulamento = _regulamentoService.BuscarRegulamento(temporada.RegulamentoId).Nome,
                RegulamentoId = temporada.RegulamentoId,
                TemporadaId = temporada.TemporadaId
                
            };

            return View(campeonatoTemporadaVm);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("TemporadaId,QuantidadeEtapas,RegulamentoId")] Temporada temporada)
        {
            ViewBag.Regulamentos = _regulamentoService.ListarRegulamentos();

            temporada.CampeonatoId = _campeonatoService.BuscarIdCampeonato(nomeUsuario);

            if (ModelState.IsValid)
            {
                try
                {
                    _temporadaService.Atualizar(temporada);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! _temporadaService.TemporadaExists(temporada.CampeonatoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                temporada.Etapas = _etapaService.ListarEtapasTemporada(temporada.TemporadaId);
                foreach (var item in temporada.Etapas)
                {
                    item.Circuito = await _circuitoService.BuscarCircuitoAsync(item.CircuitoId);
                }
                CampeonatoViewModel campeonatoTemporadaVm = new CampeonatoViewModel()
                {
                    Etapas = temporada.Etapas.OrderBy(x => x.Data).ToList(),
                    AnoTemporada = temporada.AnoTemporada,
                    QuantidadeEtapas = temporada.QuantidadeEtapas,
                    Regulamento = _regulamentoService.BuscarRegulamento(temporada.RegulamentoId).Nome
                };
                return View(campeonatoTemporadaVm);
            }
            return View(temporada);
        }

        public async Task<IActionResult> VisualizarPilotosTemporada(int anoTemporada)
        {
            TempData["anoTemporada"] = anoTemporada;

            var temporada = await _temporadaService.BuscarTemporadaAsync(campeonatoId, anoTemporada);
            var pilotosCampeonato = _pilotoService.ListarPilotosDoCampeonato(temporada.CampeonatoId);

            List<PilotoViewModel> listaPilotoCampeonatoVM = new List<PilotoViewModel>() { };
            foreach (var piloto in pilotosCampeonato)
            {
                var nomeUsuario = await _usuarioService.BuscarUsuarioAsync(piloto.UsuarioId);
                PilotoViewModel pilotoVM = new PilotoViewModel()
                {
                    NomePiloto = piloto.Nome,
                    PilotoId = piloto.PilotoId,
                    UserLogin = nomeUsuario.UserName,
                };
                listaPilotoCampeonatoVM.Add(pilotoVM);
            }
            var listaPilotosTemporadaAdicionado = _pilotoService.ListarPilotosVmTemporada(temporada.TemporadaId);
            var listaPilotosTemporadaNaoAdicionado = _pilotoService.ListarPilotosVmSemTemporada(listaPilotoCampeonatoVM, listaPilotosTemporadaAdicionado);

            ViewBag.ListaPilotosCampeonato = listaPilotoCampeonatoVM;
            ViewBag.ListaPilotosTemporadaAdicionado = listaPilotosTemporadaAdicionado;
            ViewBag.ListaPilotosTemporadaNaoAdicionado = listaPilotosTemporadaNaoAdicionado;

            return View(listaPilotoCampeonatoVM);
        }
        [HttpPost]
        public async Task<JsonResult> PostAddPilotos(List<string> idPiloto, int anoTemporada, [Bind("Id,PilotoId,TemporadaId")] PilotoTemporada pilotoTemporada)
        {
            var temporadaId = await _temporadaService.BuscarIdTemporadaAsync(nomeUsuario, anoTemporada);
            if (ModelState.IsValid)
            {
                foreach (var id in idPiloto)
                {
                    pilotoTemporada.Id = 0;
                    pilotoTemporada.TemporadaId = temporadaId;
                    pilotoTemporada.PilotoId = Convert.ToInt32(id);
                    _pilotoService.SalvarPilotoTemporada(pilotoTemporada);
                }
            }
            return new JsonResult(Ok());
        }
        [HttpPost]
        public async Task<JsonResult> PostRemoverPilotos(List<string> idPiloto, int anoTemporada)
        {
            var temporadaId = await _temporadaService.BuscarIdTemporadaAsync(nomeUsuario, anoTemporada);
            foreach (var id in idPiloto)
            {
                var pilotorRemover = await _pilotoService.BuscarPilotoTemporadaAsync(Convert.ToInt32(id), temporadaId);
                if (pilotorRemover != null)
                {
                    _pilotoService.RemoverPilotoTemporada(pilotorRemover);
                }
            }
            return new JsonResult(Ok());
        }

        
    }
}
