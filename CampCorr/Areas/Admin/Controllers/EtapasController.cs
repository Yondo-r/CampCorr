using Microsoft.AspNetCore.Mvc;
using CampCorr.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CampCorr.Models;
using CampCorr.ViewModels;
using CampCorr.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampCorr.Areas.Campeonato.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Adm")]
    public class EtapasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICampeonatoRepository _campeonatoRepository;
        private readonly ITemporadaRepository _temporadaRepository;
        private readonly IEtapaRepository _etapaRepository;
        private readonly string nomeUsuario;

        public EtapasController(AppDbContext context, SignInManager<IdentityUser> signInManager, ICampeonatoRepository campeonatoRepository, ITemporadaRepository temporadaRepository, IEtapaRepository etapaRepository)
        {
            _context = context;
            _signInManager = signInManager;
            nomeUsuario = _signInManager.Context.User.Identity.Name;
            _campeonatoRepository = campeonatoRepository;
            _temporadaRepository = temporadaRepository;
            _etapaRepository = etapaRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(int anoTemporada)
        {
            var temporada = _context.Temporadas
                .Where(x => x.CampeonatoId == _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(_signInManager.Context.User.Identity.Name)
                && x.AnoTemporada == anoTemporada)
                .FirstOrDefault();
            var etapaAtual = _context.Etapas.Where(x => x.TemporadaId == temporada.TemporadaId).Count() + 1;
            TempData["EtapaAtual"] = etapaAtual;

            CampeonatoViewModel campeonatoTemporadaVm = new CampeonatoViewModel()
            {
                Circuitos = _context.Circuitos.ToList(),
            };

            ViewData["TemporadaId"] = new SelectList(_context.Temporadas, "TemporadaId", "Ano");
            TempData["NumeroEvento"] = etapaAtual.ToString() + " de " + temporada.QuantidadeEtapas;
            TempData["anoTemporada"] = anoTemporada;
            return View(campeonatoTemporadaVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int anoTemporada, [Bind("EtapaId,Traçado,Data,NumeroEvento,TemporadaId,KartodromoId")] Etapa etapa)
        {
            if (anoTemporada != etapa.Data.Year)
            {
                ModelState.AddModelError("Data", "O ano da corrida não pode ser diferente do ano da temporada");
            }
            var temporada = _context.Temporadas
                .Where(x => x.CampeonatoId == _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(_signInManager.Context.User.Identity.Name)
                && x.AnoTemporada == anoTemporada)
                .FirstOrDefault();
            etapa.TemporadaId = _temporadaRepository.BuscarIdTemporadaPorNomeUsuario(_signInManager.Context.User.Identity.Name, etapa.Data.Year);
            //etapa.TemporadaId = temporadaId;
            var etapaAtual = Convert.ToInt32(etapa.NumeroEvento.Substring(0, 1));
            if (ModelState.IsValid)
            {
                _context.Add(etapa);
                await _context.SaveChangesAsync();

                int quantidadeEtapas = _context.Temporadas.Where(x => x.TemporadaId == etapa.TemporadaId).FirstOrDefault().QuantidadeEtapas;

                if (etapaAtual == quantidadeEtapas)
                {
                    return RedirectToAction("Edit", "Temporadas", new { ano = anoTemporada });
                }

                return RedirectToAction("Create", "Etapas", new { anoTemporada = anoTemporada });

            }
            else
            {
                var erro = ModelState.Values;
            }
            ViewData["CampeonatoId"] = new SelectList(_context.Etapas, "EtapaId", "Ano", etapa.EtapaId);
            TempData["NumeroEvento"] = etapaAtual.ToString() + " de " + temporada.QuantidadeEtapas;
            TempData["anoTemporada"] = anoTemporada;

            CampeonatoViewModel campeonatoTemporadaVm = new CampeonatoViewModel()
            {
                Circuitos = _context.Circuitos.ToList(),
                Data = etapa.Data,
                NumeroEvento = etapa.NumeroEvento,
                Traçado = etapa.Traçado

            };
            return View(campeonatoTemporadaVm);
        }

        public async Task<IActionResult> Edit(string numeroEtapa, int ano)
        {
            var etapaId = _etapaRepository.BuscarIdEtapaPorNomeUsuario(nomeUsuario, numeroEtapa, ano);
            var etapa = await _context.Etapas.FindAsync(etapaId);
            var quantidadeEtapas = _context.Temporadas.Where(x => x.TemporadaId == etapa.TemporadaId).FirstOrDefault().QuantidadeEtapas;
            var circuito = _context.Circuitos.Where(x => x.CircuitoId == etapa.KartodromoId).FirstOrDefault();
            TempData["circuitoId"] = etapa.KartodromoId;
            TempData["anoTemporada"] = ano;
            Etapa EtapaVm = new Etapa()
            {
                Traçado = etapa.Traçado,
                Data = etapa.Data,
                Kartodromo = circuito,
                EtapaId = etapaId
            };
            ViewBag.Circuitos = _context.Circuitos.ToList();
            TempData["NumeroEvento"] = numeroEtapa + " de " + quantidadeEtapas;
            return View(EtapaVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int circuitoId, int anoTemporada, [Bind("EtapaId,Traçado,Data,NumeroEvento,TemporadaId")] Etapa etapa)
        {
            etapa.TemporadaId = _temporadaRepository.BuscarIdTemporadaPorNomeUsuario(nomeUsuario, anoTemporada);
            //etapa.EtapaId = _etapaRepository.BuscarIdEtapaPorNomeUsuario(nomeUsuario, etapa.NumeroEvento.Substring(0, 1), anoTemporada);
            if (anoTemporada != etapa.Data.Year)
            {
                ModelState.AddModelError("Data", "O ano da corrida não pode ser diferente do ano da temporada");
            }
            //verifica se foi selecionado um novo kartodromo ou se será usado o mesmo
            if (etapa.KartodromoId == 0)
            {
                etapa.KartodromoId = circuitoId;
                //_context.Update(etapa);
                //await _context.SaveChangesAsync();
            }
            
                if (ModelState.IsValid)
                {
                    _context.Update(etapa);
                    await _context.SaveChangesAsync();
                }
            


            TempData["NumeroEvento"] = etapa.NumeroEvento;
            return RedirectToAction("Edit", "Temporadas", new { ano = anoTemporada });
        }

        public async Task<IActionResult> Delete(string numeroEtapa, int ano)
        {
            var etapaId = _etapaRepository.BuscarIdEtapaPorNomeUsuario(nomeUsuario, numeroEtapa.Substring(0, 1), ano);
            var etapa = await _context.Etapas.FindAsync(etapaId);
            if (etapa != null)
                _context.Etapas.Remove(etapa);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", "Temporadas", new { ano = ano });
        }

        public IActionResult AdicionarPiloto()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarPiloto(Models.Piloto piloto, int ano)
        {
            var temporadaId = _temporadaRepository.BuscarIdTemporadaPorNomeUsuario(nomeUsuario, ano);
            var temporada = await _context.Temporadas.FindAsync(temporadaId);
            if (piloto != null && temporada != null)
            {
                temporada.Pilotos.Add(piloto);
                await _context.SaveChangesAsync();
                return View();

            }
            return View();
        }

    }
}
