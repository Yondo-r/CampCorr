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
        private readonly AppDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICampeonatoRepository _campeonatoRepository;
        private readonly ITemporadaRepository _temporadaRepository;
        private readonly IEtapaRepository _etapaRepository;
        private readonly IPilotoRepository _pilotoRepository;
        private readonly IResultadoRepository _resultadoRepository;
        private readonly string nomeUsuario;
        private readonly IEquipeRepository _equipeRepository;
        private readonly IRegulamentoRepository _regulamentoRepository;

        public ResultadosController(AppDbContext context, SignInManager<IdentityUser> signInManager, ICampeonatoRepository campeonatoRepository, ITemporadaRepository temporadaRepository, IEtapaRepository etapaRepository, IPilotoRepository pilotoRepository, IResultadoRepository resultadoRepository, IEquipeRepository equipeRepository, IRegulamentoRepository regulamentoRepository)
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
            _regulamentoRepository = regulamentoRepository;
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
                if (!CalcularResultadoEtapa(listaResultadoCorrida))
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
            var temporada = await _context.Temporadas.Where(x => x.TemporadaId == temporadaId).FirstOrDefaultAsync();
            List<ResultadoCorridaViewModel> resultadoTemporadaParcial = MontaResultadoTemporadaParcial(temporadaId);
            return View(resultadoTemporadaParcial);
        }
        #region Métodos
        private List<ResultadoCorridaViewModel> MontaResultadoTemporadaParcial(int temporadaId)
        {
            List<ResultadoCorridaViewModel> tabelaResultado = new List<ResultadoCorridaViewModel>();
            List<ResultadoCorrida> resultadoTemporadaParcial = MontaListaResultadoTemporada(temporadaId);
            List<CampCorr.Models.Piloto> listaPilotos = MontaListaPilotosTemporada(temporadaId);

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
                    NomeEquipe = _equipeRepository.BuscarEquipe(listaResultadosDoPiloto[0].EtapaId, piloto.PilotoId).Nome,
                    Pontos = pontos,
                    NumeroVitorias = vitorias,
                };
                tabelaResultado.Add(resultadoPiloto);
            }
            //tabelaResultado = (List<ResultadoCorridaViewModel>)tabelaResultado.OrderByDescending(x => x.Pontos);
            return tabelaResultado;
        }

        private List<ResultadoCorrida> MontaListaResultadoTemporada(int temporadaId)
        {
            List<ResultadoCorrida> listaResultadoTemporada = new List<ResultadoCorrida>();
            var resultadoId = _context.ResultadosCorrida.Join(_context.Etapas,
                rc => rc.EtapaId,
                et => et.EtapaId, (rc, et) => new { rc, et })
                .Join(_context.Temporadas,
                Tp => Tp.et.TemporadaId,
                T => T.TemporadaId, (Tp, T) => new { Tp, T })
                .Where(x => x.T.TemporadaId == temporadaId).Select(x => x.Tp.rc.ResultadoId).ToList();
            foreach (int id in resultadoId)
            {
                listaResultadoTemporada.Add(_context.ResultadosCorrida.Where(x => x.ResultadoId == id).FirstOrDefault());
            }
            return listaResultadoTemporada;
        }

        private List<Models.Piloto> MontaListaPilotosTemporada(int temporadaId)
        {
            List<CampCorr.Models.Piloto> listaPilotos = new List<Models.Piloto>();
            var idsPilotos = _context.ResultadosCorrida.Join(_context.Etapas,
                rc => rc.EtapaId,
                et => et.EtapaId, (rc, et) => new {rc, et})
                .Join(_context.Temporadas,
                Tp => Tp.et.TemporadaId,
                T => T.TemporadaId, (Tp, T) => new {Tp, T})
                .Where(x => x.T.TemporadaId == temporadaId).Select(x => x.Tp.rc.PilotoId).Distinct().ToList();

            foreach (int id in idsPilotos)
            {
                listaPilotos.Add(_context.Pilotos.Where(x => x.PilotoId == id).FirstOrDefault());
            }

            return listaPilotos;
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
                    var nomePiloto = _pilotoRepository.BuscarPilotoPorId(resultadoCorrida.PilotoId).Nome;
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

        //ToDo Alterar estrutura para corrigir o serviço calculo
        #region ServiçoCalculo
        public bool CalcularResultadoEtapa(List<ResultadoCorrida> listaResultadoCorridas)
        {
            bool sucesso = false;
            int etapaId = listaResultadoCorridas[0].EtapaId;
            var regulamento = _regulamentoRepository.BuscarRegulamentoPorEtapaId(etapaId);
            switch (regulamento.Nome)
            {
                case "Akgp":
                    sucesso = CalcularResultadoEtapaAkgp(listaResultadoCorridas);
                    if (sucesso)
                        _etapaRepository.ConcluirEtapa(etapaId);

                    break;
                default:
                    sucesso = CalcularResultadoEtapaF1(listaResultadoCorridas);
                    break;
            }
            return sucesso;
        }
        private bool CalcularResultadoEtapaAkgp(List<ResultadoCorrida> resultadoCorridas)
        {
            //Verifica se existe apenas uma melhor volta cadastrada
            if (resultadoCorridas.Any(x => x.MelhorVolta == true) && (resultadoCorridas.Count(x => x.MelhorVolta == true) == 1))
            {
                var regrasAkgp = new RegrasAkgp();
                var listaPontuação = regrasAkgp.MontarListaPontos();
                resultadoCorridas = resultadoCorridas.OrderBy(x => x.Posicao).ToList();
                for (int i = 0; i < resultadoCorridas.Count(); i++)
                {
                    resultadoCorridas[i].Pontos = (listaPontuação[i] - resultadoCorridas[i].PontosPenalidade);
                    if (resultadoCorridas[i].MelhorVolta == true)
                    {
                        resultadoCorridas[i].Pontos += 2;
                    }
                    //_resultadoRepository.SalvarResultado(resultadoCorridas[i]);
                }
                return true;
            }
            return false;
        }
        private bool CalcularResultadoEtapaF1(List<ResultadoCorrida> resultadoCorridas)
        {
            return false;
        }

        #endregion
        #endregion
    }
}
