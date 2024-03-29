﻿using Microsoft.AspNetCore.Authorization;
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
            var listaResultadoVm = _resultadoService.MontaListaResultadoVm(etapa);
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
            ViewBag.listaResultado = _resultadoService.MontaListaResultadoVm(etapa);
            return RedirectToAction("ResultadoCorrida", new { etapaId = etapa.EtapaId });
        }
        [HttpPost]
        public async Task<IActionResult> FinalizarEtapa(int etapaId)
        {
            var listaPilotosAusentes = _resultadoService.MontaListaResultadoVm(_etapaService.BuscarEtapa(etapaId)).Where(x => x.Posicao == null).ToList();
            foreach (var pilotoAusente in listaPilotosAusentes)
            {
                AdicionaResultadoAusente(pilotoAusente);
            }

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

        

        public IActionResult AcompanharResultados(int temporadaId)
        {
            List<ResultadoCorridaViewModel> resultadoTemporadaParcial = _resultadoService.MontaResultadoTemporadaParcial(temporadaId);
            return View(resultadoTemporadaParcial);
        }

        public IActionResult ConcluirTemporada(int temporadaId)
        {
            var listaResultadoTemporada = _calculoService.CalcularResultadoTemporada(temporadaId);
            var temporada = _temporadaService.BuscarTemporada(temporadaId);
            if (temporada.Concluida == false)
            {
                _resultadoService.ConcluirTemporada(listaResultadoTemporada, temporada);
                return RedirectToAction("ResultadoTemporada", new { temporadaId = temporadaId });
            }
            return RedirectToAction("Index");


        }
        public IActionResult ResultadoTemporada(int temporadaId)
        {
            List<ResultadoCorridaViewModel> resultadoTemporada = MontaResultadoTemporada(temporadaId);
            return View(resultadoTemporada);
        }

        
        #region Métodos
        //Salva na tabela de resultados o resultado do piloto com posição 0 para que ele não receba pontos
        private void AdicionaResultadoAusente(ResultadoCorridaViewModel pilotoAusente)
        {
            ResultadoCorrida resultadoPilotoAusente = new ResultadoCorrida()
            {
                EtapaId = pilotoAusente.EtapaId,
                Posicao = 0,
                EquipeId = pilotoAusente.EquipeId,
                PilotoId = pilotoAusente.PilotoId,
            };
            _resultadoService.Salvar(resultadoPilotoAusente);
        }

        private List<ResultadoCorridaViewModel> MontaResultadoTemporada(int temporadaId)
        {
            List<ResultadoCorridaViewModel> tabelaResultado = new List<ResultadoCorridaViewModel>();
            List<ResultadoTemporada> resultadosTemporada = _resultadoService.MontaListaResultadoFinalTemporada(temporadaId);

            foreach (ResultadoTemporada resultadoTemporadaPiloto in resultadosTemporada)
            {
                ResultadoCorridaViewModel resultadoPiloto = new ResultadoCorridaViewModel()
                {
                    NomePiloto = _pilotoService.BuscarPiloto(resultadoTemporadaPiloto.PilotoId).Nome,
                    NomeEquipe = _equipeService.BuscarEquipe(resultadoTemporadaPiloto.EquipeId).Nome,
                    Pontos = resultadoTemporadaPiloto.Pontos,
                    Posicao = resultadoTemporadaPiloto.Posicao,
                    NumeroVitorias = resultadoTemporadaPiloto.NumeroVitorias
                };
                tabelaResultado.Add(resultadoPiloto);
            }


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
                    if (!posicao.Contains((int)resultadoCorrida.Posicao) || (int)resultadoCorrida.Posicao == 0)
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
                    //if (resultadoCorrida.Posicao == 0)
                    //{
                    //    mensagemErro = mensagemErro + "A posição do " + nomePiloto + " não pode ser 0" + "<br />";
                    //    ModelState.AddModelError("Posicao", "A posição do " + nomePiloto + " não pode ser 0");
                    //}
                }
            }
            TempData["erros"] = mensagemErro;
        }


        
        #endregion
    }
}
