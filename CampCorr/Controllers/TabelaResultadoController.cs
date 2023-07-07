using CampCorr.Services.Interfaces;
using CampCorr.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CampCorr.Controllers
{
    public class TabelaResultadoController : Controller
    {
        private readonly ICampeonatoService _campeonatoService;
        private readonly IResultadoService _resultadoService;
        private readonly ITemporadaService _temporadaService;
        private readonly IEtapaService _etapaService;
        private readonly IUtilitarioService _utilitarioService;
        private readonly ICircuitoService _circuitoService;

        public TabelaResultadoController(ICampeonatoService campeonatoService, IResultadoService resultadoService, ITemporadaService temporadaService, IEtapaService etapaService, IUtilitarioService utilitarioService, ICircuitoService circuitoService)
        {
            _campeonatoService = campeonatoService;
            _resultadoService = resultadoService;
            _temporadaService = temporadaService;
            _etapaService = etapaService;
            _utilitarioService = utilitarioService;
            _circuitoService = circuitoService;
        }

        public async Task<IActionResult> ResultadoAtual(string nomeCampeonato)
        {
            var campeonato = await _campeonatoService.BuscarCampeonato(await _campeonatoService.BuscarIdCampeonatoAsync(nomeCampeonato));
            var temporadaId = await _temporadaService.BuscarIdTemporadaAsync(nomeCampeonato, DateTime.Now.Year);
            ViewBag.nomeCampeonato = nomeCampeonato;
            ViewBag.logoCampeonato = _utilitarioService.MontaImagem(campeonato.Logo);
            List<ResultadoCorridaViewModel> resultadoTemporadaParcial = _resultadoService.MontaResultadoTemporadaParcial(temporadaId);
            return View(resultadoTemporadaParcial);
        }

        public async Task<IActionResult> ResultadoPorEtapa(string nomeCampeonato, string numeroEtapa, int navegacao)
        {
            var campeonato = await _campeonatoService.BuscarCampeonato(await _campeonatoService.BuscarIdCampeonatoAsync(nomeCampeonato));
            ViewBag.nomeCampeonato = nomeCampeonato.ToString();
            ViewBag.logoCampeonato = _utilitarioService.MontaImagem(campeonato.Logo);
            var temporadaId = await _temporadaService.BuscarIdTemporadaAsync(nomeCampeonato, DateTime.Now.Year);
            var etapa = _etapaService.ListarEtapasTemporada(temporadaId).FirstOrDefault();
            if (numeroEtapa != null)
            {
                etapa = await _etapaService.BuscarEtapaAsync(nomeCampeonato, _etapaService.NavegarEtapas(numeroEtapa, navegacao), DateTime.Now.Year);
            }
            
            var listaResultadoVm = _resultadoService.MontaListaResultadoVm(etapa);
            etapa.Circuito = await _circuitoService.BuscarCircuitoAsync(etapa.CircuitoId);
            ViewBag.etapa = etapa;
            ViewBag.primeiraUltimaEtapa = _etapaService.VerificaSePrimeiraOuUltimaEtapa(etapa.EtapaId);
            ViewBag.listaResultado = listaResultadoVm;
            ViewBag.numeroEtapa = etapa.NumeroEvento;

            return View();
        }
    }
}
