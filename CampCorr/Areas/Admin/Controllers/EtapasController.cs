using Microsoft.AspNetCore.Mvc;
using CampCorr.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CampCorr.Models;
using CampCorr.ViewModels;
using CampCorr.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using CampCorr.Services.Interfaces;

namespace CampCorr.Areas.Campeonato.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Adm")]
    public class EtapasController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICampeonatoService _campeonatoService;
        private readonly ITemporadaService _temporadasService;
        private readonly IEtapaService _etapaService;
        private readonly ICircuitoService _circuitoService;
        private readonly string nomeUsuario;
        private readonly int campeonatoId;

        public EtapasController(SignInManager<IdentityUser> signInManager, ITemporadaService temporadasService, ICircuitoService circuitoService, ICampeonatoService campeonatoService, IEtapaService etapaService)
        {
            _signInManager = signInManager;
            nomeUsuario = _signInManager.Context.User.Identity.Name;
            _campeonatoService = campeonatoService;
            campeonatoId = _campeonatoService.BuscarIdCampeonato(nomeUsuario);
            _temporadasService = temporadasService;
            _circuitoService = circuitoService;
            _etapaService = etapaService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(int anoTemporada)
        {
            var temporada = await _temporadasService.BuscarTemporadaAsync(campeonatoId, anoTemporada);
            var etapaAtual = _etapaService.BuscarNumeroEtapaAtual(temporada.TemporadaId);
            TempData["EtapaAtual"] = etapaAtual;

            CampeonatoViewModel campeonatoTemporadaVm = new CampeonatoViewModel()
            {
                Circuitos = _circuitoService.ListarCircuitos()
            };

            TempData["NumeroEvento"] = etapaAtual.ToString() + " de " + temporada.QuantidadeEtapas;
            TempData["anoTemporada"] = anoTemporada;
            return View(campeonatoTemporadaVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int anoTemporada, [Bind("EtapaId,Traçado,Data,NumeroEvento,TemporadaId,CircuitoId")] Etapa etapa)
        {
            if (anoTemporada != etapa.Data.Year)
            {
                ModelState.AddModelError("Data", "O ano da corrida não pode ser diferente do ano da temporada");
            }
            var temporada = await _temporadasService.BuscarTemporadaAsync(campeonatoId, anoTemporada);
            etapa.TemporadaId = await _temporadasService.BuscarIdTemporadaAsync(nomeUsuario, etapa.Data.Year);
            //etapa.TemporadaId = temporadaId;



            var etapaAtual = _etapaService.VerificaNumeroEtapaAtual(etapa.NumeroEvento, temporada.TemporadaId); //Convert.ToInt32(etapa.NumeroEvento.Substring(0, 1));
            if (ModelState.IsValid)
            {
                _etapaService.Salvar(etapa);

                int quantidadeEtapas = _etapaService.QuantidadeEtapas(temporada.TemporadaId);

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
            //ViewData["CampeonatoId"] = new SelectList(_context.Etapas, "EtapaId", "Ano", etapa.EtapaId);
            TempData["NumeroEvento"] = etapaAtual.ToString() + " de " + temporada.QuantidadeEtapas;
            TempData["anoTemporada"] = anoTemporada;

            CampeonatoViewModel campeonatoTemporadaVm = new CampeonatoViewModel()
            {
                Circuitos = _circuitoService.ListarCircuitos(),
                Data = etapa.Data,
                NumeroEvento = etapa.NumeroEvento,
                Traçado = etapa.Traçado

            };
            return View(campeonatoTemporadaVm);
        }

        public async Task<IActionResult> Edit(string numeroEtapa, int ano)
        {
            var etapa = await _etapaService.BuscarEtapaAsync(nomeUsuario, numeroEtapa, ano);
            var quantidadeEtapas = _etapaService.QuantidadeEtapas(etapa.TemporadaId);
            var circuito = _circuitoService.ListarCircuitos().Where(x => x.CircuitoId == etapa.CircuitoId).FirstOrDefault();
            TempData["circuitoId"] = etapa.CircuitoId;
            TempData["anoTemporada"] = ano;
            Etapa EtapaVm = new Etapa()
            {
                Traçado = etapa.Traçado,
                Data = etapa.Data,
                Circuito = circuito,
                EtapaId = etapa.EtapaId
            };
            ViewBag.Circuitos = _circuitoService.ListarCircuitos();
            TempData["NumeroEvento"] = numeroEtapa + " de " + quantidadeEtapas;
            return View(EtapaVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int anoTemporada,  Etapa etapa)
        {
            etapa.TemporadaId = await _temporadasService.BuscarIdTemporadaAsync(nomeUsuario, anoTemporada);
            if (anoTemporada != etapa.Data.Year)
            {
                ModelState.AddModelError("Data", "O ano da corrida não pode ser diferente do ano da temporada");
            }

            if (ModelState.IsValid)
            {
                _etapaService.Atualizar(etapa);
            }

            TempData["NumeroEvento"] = etapa.NumeroEvento;
            return RedirectToAction("Edit", "Temporadas", new { ano = anoTemporada });
        }

        public async Task<IActionResult> Delete(string numeroEtapa, int ano)
        {
            var etapa = await _etapaService.BuscarEtapaAsync(nomeUsuario, numeroEtapa.Substring(0, 1), ano);
            if (etapa != null)
            {
                _etapaService.Remover(etapa);
            }
            return RedirectToAction("Edit", "Temporadas", new { ano = ano });
        }

        //public IActionResult AdicionarPiloto()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AdicionarPiloto(Models.Piloto piloto, int ano)
        //{
        //    var temporada = await _temporadasService.BuscarTemporadaAsync(campeonatoId, ano);
        //    if (piloto != null && temporada != null)
        //    {

        //        temporada.Pilotos.Add(piloto);
        //        await _context.SaveChangesAsync();
        //        return View();

        //    }
        //    return View();
        //}

    }
}
