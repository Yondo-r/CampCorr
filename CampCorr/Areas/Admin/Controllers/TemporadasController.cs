using Microsoft.AspNetCore.Mvc;
using CampCorr.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using CampCorr.ViewModels;

namespace CampCorr.Areas.Campeonato.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Adm")]
    public class TemporadasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly string nomeUsuario;
        private readonly IPilotoRepository _pilotoRepository;
        private readonly ICampeonatoRepository _campeonatoRepository;
        private readonly ITemporadaRepository _temporadaRepository;

        public TemporadasController(AppDbContext context, SignInManager<IdentityUser> signInManager, IPilotoRepository pilotoRepository, ICampeonatoRepository campeonatoRepository, ITemporadaRepository temporadaRepository)
        {
            _context = context;
            _signInManager = signInManager;
            _pilotoRepository = pilotoRepository;
            nomeUsuario = signInManager.Context.User.Identity.Name;
            _campeonatoRepository = campeonatoRepository;
            _temporadaRepository = temporadaRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(int? campeonatoId)
        {
            TempData["campeonatoId"] = campeonatoId;
            ViewData["CampeonatoId"] = new SelectList(_context.Campeonatos, "CampeonatoId", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TemporadaId,QuantidadeEtapas,RegulamentoId")] Temporada temporada)
        {
            temporada.CampeonatoId = _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(_signInManager.Context.User.Identity.Name);
             //campeonato = _context.Campeonatos.Where(x => x.CampeonatoId == CampeonatoId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                _context.Add(temporada);

                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { temporadaId = temporada.TemporadaId });
            }
            ViewData["CampeonatoId"] = new SelectList(_context.Campeonatos, "CampeonatoId", "Nome", temporada.TemporadaId);
            return View(temporada);
        }

        public async Task<IActionResult> Edit(int ano)
        {
            if (_context.Campeonatos == null)
            {
                return NotFound();
            }
            var temporadaId = _temporadaRepository.BuscarIdTemporadaPorNomeUsuario(_signInManager.Context.User.Identity.Name, ano);
            var temporada = await _context.Temporadas.FindAsync(temporadaId);
            if (temporada == null)
            {
                return NotFound();
            }

            temporada.Etapas = _context.Etapas.Where(x => x.TemporadaId == temporadaId).ToList();
            foreach (var item in temporada.Etapas)
            {
                item.Kartodromo = _context.Circuitos.Where(x => x.CircuitoId == item.KartodromoId).FirstOrDefault();
            }
            CampeonatoViewModel campeonatoTemporadaVm = new CampeonatoViewModel()
            {
                Etapas = temporada.Etapas.OrderBy(x => x.Data).ToList(),
                AnoTemporada = temporada.AnoTemporada,
                QuantidadeEtapas = temporada.QuantidadeEtapas,
                Regulamento = _context.Regulamentos.Where(x => x.RegulamentoId == temporada.RegulamentoId).FirstOrDefault().Descricao,
            };

            return View(campeonatoTemporadaVm);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int temporadaId, [Bind("TemporadaId,QuantidadeEtapas,RegulamentoId")] Temporada temporada)
        {
            if (temporadaId != temporada.CampeonatoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(temporada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemporadaExists(temporada.CampeonatoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(temporada);
        }

        public async Task<IActionResult> VisualizarPilotosTemporada(int anoTemporada)
        {
            TempData["anoTemporada"] = anoTemporada;

            var temporada = await _context.Temporadas
                .Where(x => x.CampeonatoId == _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(nomeUsuario)
                && x.AnoTemporada == anoTemporada)
                .FirstOrDefaultAsync();
            var pilotosCampeonato = _pilotoRepository.BuscarPilotosCadastradosCampeonato(temporada.CampeonatoId);

            List<PilotoViewModel> listaPilotoCampeonatoVM = new List<PilotoViewModel>() { };
            foreach (var piloto in pilotosCampeonato)
            {
                PilotoViewModel pilotoVM = new PilotoViewModel()
                {
                    NomePiloto = piloto.Nome,
                    PilotoId = piloto.PilotoId,
                    UserLogin = _context.Users.Where(x => x.Id == piloto.UsuarioId).Select(x => x.UserName).FirstOrDefault(),
                };
                listaPilotoCampeonatoVM.Add(pilotoVM);
            }
            var listaPilotosTemporadaAdicionado = _pilotoRepository.PreencherListaDePilotosTemporada(temporada.TemporadaId);
            var listaPilotosTemporadaNaoAdicionado = _pilotoRepository.PreencherListaDePilotosNaoAdicionadosTemporada(listaPilotoCampeonatoVM, listaPilotosTemporadaAdicionado);

            ViewBag.ListaPilotosCampeonato = listaPilotoCampeonatoVM;
            ViewBag.ListaPilotosTemporadaAdicionado = listaPilotosTemporadaAdicionado;
            ViewBag.ListaPilotosTemporadaNaoAdicionado = listaPilotosTemporadaNaoAdicionado;

            return View(listaPilotoCampeonatoVM);
        }
        [HttpPost]
        public async Task<JsonResult> PostAddPilotos(List<string> idPiloto, int anoTemporada, [Bind("Id,PilotoId,TemporadaId")] PilotoTemporada pilotoTemporada)
        {
            var temporadaId = _temporadaRepository.BuscarIdTemporadaPorNomeUsuario(nomeUsuario, anoTemporada);
            if (ModelState.IsValid)
            {
                foreach (var id in idPiloto)
                {
                    pilotoTemporada.Id = 0;
                    pilotoTemporada.TemporadaId = temporadaId;
                    pilotoTemporada.PilotoId = Convert.ToInt32(id);
                    _context.Add(pilotoTemporada);
                    await _context.SaveChangesAsync();
                }
            }
            return new JsonResult(Ok());
        }
        [HttpPost]
        public async Task<JsonResult> PostRemoverPilotos(List<string> idPiloto, int anoTemporada)
        {
            var temporadaId = _temporadaRepository.BuscarIdTemporadaPorNomeUsuario(nomeUsuario, anoTemporada);
            foreach (var id in idPiloto)
            {
                var pilotorRemover = await _context.PilotosTemporadas.Where(x => x.PilotoId == Convert.ToInt32(id) && x.TemporadaId == temporadaId).FirstOrDefaultAsync();
                if (pilotorRemover != null)
                {
                    _context.Remove(pilotorRemover);
                    await _context.SaveChangesAsync();
                }
            }
            return new JsonResult(Ok());
        }

        private bool TemporadaExists(int id)
        {
            return _context.Temporadas.Any(e => e.TemporadaId == id);
        }
    }
}
