using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampCorr.Context;
using Microsoft.AspNetCore.Identity;
using CampCorr.Repositories.Interfaces;
using CampCorr.Models;
using CampCorr.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CampCorr.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Adm")]
    public class ResultadosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICampeonatoRepository _campeonatoRepository;
        private readonly ITemporadaRepository _temporadaRepository;
        private readonly IEtapaRepository _etapaRepository;
        private readonly IPilotoRepository _pilotoRepository;
        private readonly IResultadoRepository _resultadoRepository;
        private readonly string nomeUsuario;
        private readonly IEquipeRepository _equipeRepository;

        public ResultadosController(AppDbContext context, SignInManager<IdentityUser> signInManager, ICampeonatoRepository campeonatoRepository, ITemporadaRepository temporadaRepository, IEtapaRepository etapaRepository, IPilotoRepository pilotoRepository, IResultadoRepository resultadoRepository, IEquipeRepository equipeRepository)
        {
            _context = context;
            _signInManager = signInManager;
            _campeonatoRepository = campeonatoRepository;
            _temporadaRepository = temporadaRepository;
            _etapaRepository = etapaRepository;
            nomeUsuario = _signInManager.Context.User.Identity.Name;
            _pilotoRepository = pilotoRepository;
            _resultadoRepository = resultadoRepository;
            _equipeRepository = equipeRepository;
        }

        //Exibir lista com as temporadas. Ao clicar na lista abrirá outra lista com as etapas. A lista de etapa irá mostrar se a etapa está ou não concluída.
        //Caso seja clicado em uma etapa concluída, será exibido o resultado da etapa.
        //Caso a etapa não esteja concluída, será exibido um botão para o usuário poder inserir o restultado da etapa
        public IActionResult Index()
        {
            var idCampeonato = _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(nomeUsuario);
            var listaTemporada = _context.Temporadas.Where(x => x.CampeonatoId == idCampeonato).ToList();
            var listaEtapa = _etapaRepository.BuscarListaEtapasCampeonato(idCampeonato);

            foreach (var etapa in listaEtapa)
            {
                etapa.Kartodromo = _etapaRepository.BuscarKartodromo(etapa.KartodromoId);
            }
            
            ViewBag.listaEtapas = listaEtapa;
            ViewBag.listaTemporadas = listaTemporada;
            return View();
        }
        [HttpPost]
        public IActionResult ResultadoCorrida(Etapa etapa)
        {
            //encriptar id para dificultar a alteração de resultados
            
            if (!_etapaRepository.ValidarEtapa(etapa.EtapaId, etapa.Data))
            {
                ModelState.AddModelError("Dados inválidos", "Não foi possível Continuar");
            }
            if (ModelState.IsValid)
            {
                etapa.TemporadaId = _context.Etapas.Where(x => x.EtapaId == etapa.EtapaId).Select(x => x.TemporadaId).FirstOrDefault();
                var listaEquipes = _equipeRepository.PreencherListaEquipesAdicionadas(etapa.TemporadaId);
                ViewBag.ListaEquipe = listaEquipes;
                etapa = _etapaRepository.BuscarEtapaPorId(etapa.EtapaId);
                var listaResultadoVm = MontaListaResultadoVm(etapa);
                var listaResultado = MontaListaResultado(listaResultadoVm);
                ViewBag.listaResultado = listaResultado;
                return View(listaResultadoVm);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarResultado( 
            [Bind("ResultadoId,PilotoId,EquipeId,Posicao,MelhorVolta,PosicaoLargada,TempoMelhorVolta,TempoTotal,PontosPenalidade,DescricaoPenalidade")] 
        ResultadoCorrida resultadoCorrida)
        {

            //Pega os dados, faz o cálculo e salva no banco.
            var etapa = _etapaRepository.BuscarEtapaPorId(resultadoCorrida.EtapaId);
            var resultadoVm = _resultadoRepository.BuscarPilotoResultadoEtapa(resultadoCorrida.EtapaId, resultadoCorrida.PilotoId);
            if (ModelState.IsValid)
            {
                _context.Update(resultadoCorrida);
                await _context.SaveChangesAsync();
            }
            return View(resultadoVm);
            //return RedirectToAction("ResultadoCorrida", new { etapa });
        }

        //public ActionResult PostModalResultado(int pilotoId, int etapaId)
        //{
        //    var resultado = _resultadoRepository.BuscarPilotoResultadoEtapa(etapaId, pilotoId); //_context.ResultadosCorrida.Where(x => x.PilotoId == pilotoId && x.EtapaId == etapaId).FirstOrDefault();
        //    return PartialView("_ResultadoModalForm", resultado);
        //}

        private List<ResultadoCorrida> MontaListaResultado(List<ResultadoCorridaViewModel> resultadoVm)
        {
            List<ResultadoCorrida> listaResultado = new List<ResultadoCorrida>();
            foreach (var resultado in resultadoVm)
            {
                var resultadoCorrida = new ResultadoCorrida();
                
                resultadoCorrida.PilotoId = resultado.PilotoId;
                resultadoCorrida.EtapaId = resultado.EtapaId;
                resultadoCorrida.Posicao = resultado.Posicao;
                resultadoCorrida.EquipeId = resultado.EquipeId;
                resultadoCorrida.MelhorVolta = resultado.MelhorVolta;
                resultadoCorrida.PosicaoLargada = resultado.PosicaoLargada;
                resultadoCorrida.TempoMelhorVolta = resultado.TempoMelhorVolta;
                resultadoCorrida.TempoTotal = resultado.TempoTotal;
                resultadoCorrida.PontosPenalidade = resultado.PontosPenalidade;
                resultadoCorrida.DescricaoPenalidade = resultado.DescricaoPenalidade;
                
                listaResultado.Add(resultadoCorrida);
            }
            return listaResultado;
        }

        private List<ResultadoCorridaViewModel> MontaListaResultadoVm(Etapa etapa)
        {
            List<ResultadoCorridaViewModel> listaResultadoVm = new List<ResultadoCorridaViewModel>();
            List<PilotoViewModel> listaPilotos = _pilotoRepository.PreencherListaDePilotosTemporada(etapa.TemporadaId);

            foreach (var piloto in listaPilotos)
            {
                var resultado = _resultadoRepository.BuscarPilotoResultadoEtapa(etapa.EtapaId, piloto.PilotoId);
                listaResultadoVm.Add(resultado);
            }
            
            return listaResultadoVm;
        }
    }
}
