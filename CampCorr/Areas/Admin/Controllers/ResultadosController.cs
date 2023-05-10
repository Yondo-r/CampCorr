using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampCorr.Context;
using Microsoft.AspNetCore.Identity;
using CampCorr.Repositories.Interfaces;
using CampCorr.Models;
using CampCorr.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Data;

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

        public IActionResult ResultadoCorrida(int etapaId)
        {
            //encriptar id para dificultar a alteração de resultados

            var etapa = _context.Etapas.Where(x => x.EtapaId == etapaId).FirstOrDefault();

            var listaEquipes = _equipeRepository.PreencherListaEquipesAdicionadas(etapa.TemporadaId);
            ViewBag.ListaEquipe = listaEquipes;
            etapa = _etapaRepository.BuscarEtapaPorId(etapa.EtapaId);
            var listaResultadoVm = MontaListaResultadoVm(etapa);
            ViewBag.listaResultado = listaResultadoVm;
            return View();
        }


        public async Task<IActionResult> EditarResultado(string tempoMelhorVolta, string tempoTotal,
            [Bind("ResultadoId,PilotoId,EquipeId,EtapaId,Posicao,MelhorVolta,PosicaoLargada,TotalVoltas,PontosPenalidade,DescricaoPenalidade")]
        ResultadoCorrida resultadoCorrida)
        {
            resultadoCorrida.TempoMelhorVolta = string.IsNullOrEmpty(tempoMelhorVolta) ? default : TimeSpan.ParseExact(tempoMelhorVolta, "mm':'ss':'fff", CultureInfo.InvariantCulture);
            resultadoCorrida.TempoTotal = string.IsNullOrEmpty(tempoTotal) ? TimeSpan.Parse("0") : TimeSpan.ParseExact(tempoTotal, "hh':'mm':'ss':'fff", CultureInfo.InvariantCulture);
            if (resultadoCorrida.ResultadoId == 0)
            {
                resultadoCorrida.ResultadoId = await _context.ResultadosCorrida
                    .Where(x => x.EtapaId == resultadoCorrida.EtapaId && x.PilotoId == resultadoCorrida.PilotoId)
                    .Select(x => x.ResultadoId).FirstOrDefaultAsync();
            }
            //Pega os dados, faz o cálculo e salva no banco.
            var etapa = _etapaRepository.BuscarEtapaPorId(resultadoCorrida.EtapaId);
            if (ModelState.IsValid)
            {
                _context.Update(resultadoCorrida);
                await _context.SaveChangesAsync();
            }
            ViewBag.listaResultado = MontaListaResultadoVm(etapa);
            return RedirectToAction("ResultadoCorrida", new { etapaId = etapa.EtapaId });
        }
        [HttpPost]
        public async Task<IActionResult> FinalizarEtapa(int etapaId)
        {
            List<ResultadoCorrida> listaResultadoCorrida = await _context.ResultadosCorrida.Where(x => x.EtapaId.Equals(etapaId)).ToListAsync();
            ValidarResultadoParaFinalizarEtapa(listaResultadoCorrida);
            if (ModelState.IsValid)
            {
                foreach (ResultadoCorrida resultadoCorrida in listaResultadoCorrida)
                {
                    if (!resultadoCorrida.PontosPenalidade.HasValue)
                    {
                        resultadoCorrida.PontosPenalidade = 0;
                    }
                    _context.Update(resultadoCorrida);
                    //await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("ResultadoCorrida", new { etapaId = etapaId });
        }

        private void ValidarResultadoParaFinalizarEtapa(List<ResultadoCorrida> listaResultadoCorrida)
        {
            string mensagemErro = "";
            int quantidadeErros = 0;
            foreach (var resultadoCorrida in listaResultadoCorrida)
            {
                var nomePiloto = _pilotoRepository.BuscarPilotoPorId(resultadoCorrida.PilotoId).Nome;
                if (!resultadoCorrida.Posicao.HasValue)
                {
                    mensagemErro = mensagemErro + "É necessário adicionar a posição do(a) piloto(a) " + nomePiloto + "<br />";
                    ModelState.AddModelError("Posicao", "Não há posição para o piloto + nomePiloto");
                }
            }
            TempData["erros"] = mensagemErro;
            TempData["quantidadeErros"] = quantidadeErros;
        }

        private ResultadoCorrida MontaResultado(int etapaId, int pilotoId)
        {
            ResultadoCorrida resultado = new ResultadoCorrida()
            {
                EtapaId = etapaId,
                PilotoId = pilotoId,
                EquipeId = _equipeRepository.BuscarEquipe(etapaId, pilotoId).EquipeId
            };
            return resultado;

        }
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
