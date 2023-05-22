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
using CampCorr.Services.Interfaces;
using CampCorr.Negocios;

namespace CampCorr.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Adm")]
    public class ResultadosController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICampeonatoService _campeonatoService;
        private readonly ITemporadaService _temporadaService;
        private readonly IEtapaService _etapaService;
        private readonly ICircuitoService _circuitoService;
        private readonly IRegulamentoService _regulamentoService;
        private readonly IPilotoService _pilotoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IEquipeService _equipeService;
        private readonly IResultadoService _resultadoService;
        private readonly ICalculoService _calculoService;
        private readonly string nomeUsuario;
        private readonly int campeonatoId;

        public ResultadosController(SignInManager<IdentityUser> signInManager, ICampeonatoService campeonatoService, ITemporadaService temporadaService, IEtapaService etapaService, ICircuitoService circuitoService, IRegulamentoService regulamentoService, IPilotoService pilotoService, IUsuarioService usuarioService, IEquipeService equipeService, ICalculoService calculoService, IResultadoService resultadoService)
        {
            _signInManager = signInManager;
            nomeUsuario = _signInManager.Context.User.Identity.Name;
            _campeonatoService = campeonatoService;
            _temporadaService = temporadaService;
            _etapaService = etapaService;
            _circuitoService = circuitoService;
            _regulamentoService = regulamentoService;
            _pilotoService = pilotoService;
            _usuarioService = usuarioService;
            campeonatoId = _campeonatoService.BuscarIdCampeonato(nomeUsuario);
            _equipeService = equipeService;
            _calculoService = calculoService;
            _resultadoService = resultadoService;
        }

        //Exibir lista com as temporadas. Ao clicar na lista abrirá outra lista com as etapas. A lista de etapa irá mostrar se a etapa está ou não concluída.
        //Caso seja clicado em uma etapa concluída, será exibido o resultado da etapa.
        //Caso a etapa não esteja concluída, será exibido um botão para o usuário poder inserir o restultado da etapa
        public async Task<IActionResult> Index()
        {
            var listaTemporada = _temporadaService.ListarTemporadasDoCampeonato(campeonatoId);
            var listaEtapa = _etapaService.ListarEtapasCampeonato(campeonatoId);

            foreach (var etapa in listaEtapa)
            {
                etapa.Circuito = await _circuitoService.BuscarCircuitoAsync(etapa.CircuitoId);
            }

            ViewBag.listaEtapas = listaEtapa;
            ViewBag.listaTemporadas = listaTemporada;
            return View();
        }

        public IActionResult ResultadoCorrida(int etapaId)
        {
            //encriptar id para dificultar a alteração de resultados

            var etapa = _etapaService.BuscarEtapa(etapaId);

            var listaEquipes = _equipeService.ListaEquipesTemporada(etapa.TemporadaId);
            ViewBag.ListaEquipe = listaEquipes;
            ViewBag.etapaConcluida = etapa.Concluido;
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
                resultadoCorrida.ResultadoId = await _resultadoService.BuscarResultadoIdAsync(resultadoCorrida.EtapaId, resultadoCorrida.PilotoId);
            }
            //Pega os dados, faz o cálculo e salva no banco.
            var etapa = await _etapaService.BuscarEtapaAsync(resultadoCorrida.EtapaId);
            if (ModelState.IsValid)
            {
                _resultadoService.Salvar(resultadoCorrida);
            }
            ViewBag.listaResultado = MontaListaResultadoVm(etapa);
            return RedirectToAction("ResultadoCorrida", new { etapaId = etapa.EtapaId });
        }
        [HttpPost]
        public async Task<IActionResult> FinalizarEtapa(int etapaId)
        {
            List<ResultadoCorrida> listaResultadoCorrida = await _resultadoService.ListarResultadoEtapa(etapaId);
            ValidarPosicaoParaFinalizarEtapa(listaResultadoCorrida);
            if (ModelState.IsValid)
            {
                foreach (ResultadoCorrida resultadoCorrida in listaResultadoCorrida)
                {
                    if (!resultadoCorrida.PontosPenalidade.HasValue)
                    {
                        resultadoCorrida.PontosPenalidade = 0;
                    }
                }
                if (!_calculoService.CalcularResultadoEtapa(listaResultadoCorrida))
                {
                    if (!listaResultadoCorrida.Any(x => x.MelhorVolta == true))
                    {
                        TempData["erros"] = "É necessário informar qual piloto fez a melhor volta";
                    }
                    else if (listaResultadoCorrida.Count(x => x.MelhorVolta == true) > 1)
                    {
                        TempData["erros"] = "Existe mais de um piloto com a melhor volta";
                    }
                    else
                    {
                        TempData["erros"] = "Houve um erro ao processar sua solicitação";
                    }
                }
            }
            return RedirectToAction("ResultadoCorrida", new { etapaId = etapaId });
        }

        public async Task<IActionResult> AcompanharResultados(int temporadaId)
        {
            var temporada = await _temporadaService.BuscarTemporadaAsync(temporadaId);
            List<ResultadoCorridaViewModel> resultadoTemporadaParcial = MontaResultadoTemporadaParcial(temporadaId);
            return View(resultadoTemporadaParcial);
        }
        #region Métodos
        private List<ResultadoCorridaViewModel> MontaResultadoTemporadaParcial(int temporadaId)
        {
            List<ResultadoCorridaViewModel> tabelaResultado = new List<ResultadoCorridaViewModel>();
            List<ResultadoCorrida> resultadoTemporadaParcial = _resultadoService.MontaListaResultadoTemporada(temporadaId);
            List<CampCorr.Models.Piloto> listaPilotos = _pilotoService.MontaListaPilotosTemporada(temporadaId);

            foreach (var piloto in listaPilotos)
            {
                int pontos = 0;
                int vitorias = 0;
                var listaResultadosDoPiloto = resultadoTemporadaParcial.Where(x => x.PilotoId == piloto.PilotoId).ToList();

                //Soma os pontos do piloto
                foreach (var resultadoDoPiloto in listaResultadosDoPiloto)
                {
                    pontos += (int)resultadoDoPiloto.Pontos;

                    //Verifica quantas vitórias o piloto possui. 
                    if (resultadoDoPiloto.Posicao == 1)
                    {
                        vitorias++;
                    }
                }
                ResultadoCorridaViewModel resultadoPiloto = new ResultadoCorridaViewModel()
                {
                    NomePiloto = piloto.Nome,
                    NomeEquipe = _equipeService.BuscarEquipeDoPiloto(listaResultadosDoPiloto[0].EtapaId, piloto.PilotoId).Nome,
                    Pontos = pontos,
                    NumeroVitorias = vitorias,
                };
                tabelaResultado.Add(resultadoPiloto);
            }
            //tabelaResultado = (List<ResultadoCorridaViewModel>)tabelaResultado.OrderByDescending(x => x.Pontos);
            return tabelaResultado;
        }

        

        

        private void ValidarPosicaoParaFinalizarEtapa(List<ResultadoCorrida> listaResultadoCorrida)
        {
            string mensagemErro = "";
            if (listaResultadoCorrida.Count() == 0)
            {
                mensagemErro = mensagemErro + "Não foram adicionados resultados nessa corrida! " + "<br />";
                ModelState.AddModelError("Posicao", "Não foram cadastrados resultados!");
            }
            else
            {
                List<int> posicao = new List<int>();
                foreach (var resultadoCorrida in listaResultadoCorrida)
                {
                    var nomePiloto = _pilotoService.BuscarPiloto(resultadoCorrida.PilotoId).Nome;
                    //Verifica se não tem dois pilotos cadastrados na mesma posição
                    if (!posicao.Contains((int)resultadoCorrida.Posicao))
                    {
                        posicao.Add((int)resultadoCorrida.Posicao);
                    }
                    else
                    {
                        mensagemErro = mensagemErro + "Existe mais de um piloto cadastrado na posição " + resultadoCorrida.Posicao.ToString() + "<br />";
                        ModelState.AddModelError("Posicao", "Existe mais de um piloto cadastrado na posição " + resultadoCorrida.Posicao.ToString());
                    }
                    if (!resultadoCorrida.Posicao.HasValue)
                    {
                        mensagemErro = mensagemErro + "É necessário adicionar a posição do(a) piloto(a) " + nomePiloto + "<br />";
                        ModelState.AddModelError("Posicao", "Não há posição para o piloto" + nomePiloto);
                    }
                    if (resultadoCorrida.Posicao == 0)
                    {
                        mensagemErro = mensagemErro + "A posição do " + nomePiloto + " não pode ser 0" + "<br />";
                        ModelState.AddModelError("Posicao", "A posição do " + nomePiloto + " não pode ser 0");
                    }
                }
            }
            TempData["erros"] = mensagemErro;
        }

        //private ResultadoCorrida MontaResultado(int etapaId, int pilotoId)
        //{
        //    ResultadoCorrida resultado = new ResultadoCorrida()
        //    {
        //        EtapaId = etapaId,
        //        PilotoId = pilotoId,
        //        EquipeId = _equipeService.BuscarEquipeDoPiloto(etapaId, pilotoId).EquipeId
        //    };
        //    return resultado;

        //}
        //private List<ResultadoCorrida> MontaListaResultado(List<ResultadoCorridaViewModel> resultadoVm)
        //{
        //    List<ResultadoCorrida> listaResultado = new List<ResultadoCorrida>();
        //    foreach (var resultado in resultadoVm)
        //    {
        //        var resultadoCorrida = new ResultadoCorrida();

        //        resultadoCorrida.PilotoId = resultado.PilotoId;
        //        resultadoCorrida.EtapaId = resultado.EtapaId;
        //        resultadoCorrida.Posicao = resultado.Posicao;
        //        resultadoCorrida.EquipeId = resultado.EquipeId;
        //        resultadoCorrida.MelhorVolta = resultado.MelhorVolta;
        //        resultadoCorrida.PosicaoLargada = resultado.PosicaoLargada;
        //        resultadoCorrida.TempoMelhorVolta = resultado.TempoMelhorVolta;
        //        resultadoCorrida.TempoTotal = resultado.TempoTotal;
        //        resultadoCorrida.PontosPenalidade = resultado.PontosPenalidade;
        //        resultadoCorrida.DescricaoPenalidade = resultado.DescricaoPenalidade;

        //        listaResultado.Add(resultadoCorrida);
        //    }
        //    return listaResultado;
        //}

        private List<ResultadoCorridaViewModel> MontaListaResultadoVm(Etapa etapa)
        {
            List<ResultadoCorridaViewModel> listaResultadoVm = new List<ResultadoCorridaViewModel>();
            List<PilotoViewModel> listaPilotos = _pilotoService.ListarPilotosVmTemporada(etapa.TemporadaId);

            foreach (var piloto in listaPilotos)
            {
                var resultado =  _resultadoService.BuscarPilotoResultadoEtapa(etapa.EtapaId, piloto.PilotoId);
                listaResultadoVm.Add(resultado);
            }

            return listaResultadoVm;
        }
        #endregion
    }
}
