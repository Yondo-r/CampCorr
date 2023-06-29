using CampCorr.Models;
using CampCorr.Repositories;
using CampCorr.Services.Interfaces;
using CampCorr.Negocios;
using CampCorr.Context;
using CampCorr.Repositories.Interfaces;
using CampCorr.ViewModels;

namespace CampCorr.Services
{
    public class CalculoService : ICalculoService
    {

        private readonly IRegulamentoRepository _regulamentoRepository;
        private readonly IEtapaRepository _etapaRepository;
        private readonly ITemporadaRepository _temporadaRepository;
        private readonly IPilotoRepository _pilotoRepository;
        private readonly IResultadoRepository _resultadoRepository;
        private readonly IEtapaService _etapaService;

        public CalculoService(IRegulamentoRepository regulamentoRepository, IEtapaRepository etapaRepository, ITemporadaRepository temporadaRepository, IPilotoRepository pilotoRepository, IResultadoRepository resultadoRepository, IEtapaService etapaService)
        {
            _regulamentoRepository = regulamentoRepository;
            _etapaRepository = etapaRepository;
            _temporadaRepository = temporadaRepository;
            _pilotoRepository = pilotoRepository;
            _resultadoRepository = resultadoRepository;
            _etapaService = etapaService;
        }

        public bool CalcularResultadoEtapa(List<ResultadoCorrida> listaResultadoCorridas)
        {
            bool resultado = false;
            int etapaId = listaResultadoCorridas[0].EtapaId;
            var regulamento = _regulamentoRepository.BuscarRegulamentoPorEtapaId(etapaId);
            switch (regulamento.Nome)
            {
                case "Akgp":
                    resultado = CalcularResultadoEtapaAkgp(listaResultadoCorridas);
                    if (resultado)
                        _etapaRepository.ConcluirEtapa(etapaId);
                    break;
                default:
                    resultado = CalcularResultadoEtapaF1(listaResultadoCorridas);
                    break;
            }
            return resultado;
        }
        public List<ResultadoTemporada> CalcularResultadoTemporada(int temporadaId)
        {
            List<ResultadoTemporada> resultado;
            var temporada = _temporadaRepository.BuscarTemporada(temporadaId);
            var regulamento = _regulamentoRepository.BuscarRegulamento(temporada.RegulamentoId);

            switch (regulamento.Nome)
            {
                case "Akgp":
                    resultado = CalcularResultadoTemporadaAkgp(temporada);

                    break;
                default:
                    resultado = CalcularResultadoTemporadaF1(temporada);
                    break;
            }

            return resultado;
        }

        private List<ResultadoTemporada> CalcularResultadoTemporadaAkgp(Temporada temporada)
        {
            List<ResultadoTemporada> listaResultadoEtapasPorPiloto = new List<ResultadoTemporada>();
            List<PilotoViewModel> listaPilotos = _pilotoRepository.PreencherListaDePilotosTemporada(temporada.TemporadaId);

            //pra cada piloto na lista, deverá ser montado um resultado da temporada seguindo a regra da akgp
            //
            //⦁	Para efeito de classificação final será descartado os dois piores resultado do PILOTO durante o campeonato,
            //salvo as etapas que o PILOTO recebeu advertência/punição aplicada pela direção de prova do kartódromo.
            foreach (var piloto in listaPilotos)
            {
                int resultadosRemover = 2;
                List<ResultadoCorrida> resultadosDoPiloto = _resultadoRepository.ListarResultadoEtapa(temporada.TemporadaId, piloto.PilotoId);
                //Ordena os resultados por pontos
                resultadosDoPiloto = resultadosDoPiloto.OrderBy(x => x.Pontos).ToList();
                //Retira os 2 piores resultados que não tenha advertência ou punição
                for (int i = 0; i < resultadosRemover; i++)
                {
                    if (resultadosDoPiloto[i].Advertencia == false && resultadosDoPiloto[i].PontosPenalidade == 0)
                    {
                        resultadosDoPiloto.Remove(resultadosDoPiloto[0]);
                    }
                    else
                    {
                        resultadosRemover++;
                    }
                }
                piloto.ResultadosPiloto = resultadosDoPiloto;
                ResultadoTemporada resultadoTemporadaPiloto = new ResultadoTemporada();
                foreach (var resultado in resultadosDoPiloto)
                {
                    int vitorias = 0;
                    if (resultado.Posicao == 1)
                        vitorias = 1;
                    resultadoTemporadaPiloto.NumeroVitorias += vitorias;
                    resultadoTemporadaPiloto.Pontos += (int)resultado.Pontos;
                }
                resultadoTemporadaPiloto.TemporadaId = temporada.TemporadaId;
                resultadoTemporadaPiloto.PilotoId = resultadosDoPiloto[0].PilotoId;
                resultadoTemporadaPiloto.EquipeId = resultadosDoPiloto[0].EquipeId;

                listaResultadoEtapasPorPiloto.Add(resultadoTemporadaPiloto);
            }
            listaResultadoEtapasPorPiloto = OrdernarResultadoAkgp(listaResultadoEtapasPorPiloto, listaPilotos); //listaResultadoEtapasPorPiloto.OrderByDescending(x => x.Pontos).ThenByDescending(x => x.NumeroVitorias).ToList();



            return listaResultadoEtapasPorPiloto;
        }

        private List<ResultadoTemporada> OrdernarResultadoAkgp(List<ResultadoTemporada> listaResultadoEtapasPorPiloto, List<PilotoViewModel> listaPilotos)
        {
            List<ResultadoCorrida> resultadoOrdenado = new List<ResultadoCorrida>();
            foreach (var piloto in listaPilotos)
            {
                foreach (var resultadoPiloto in piloto.ResultadosPiloto)
                {
                    ResultadoCorrida result = new ResultadoCorrida()
                    {
                        Advertencia = resultadoPiloto.Advertencia,
                        DescricaoPenalidade = resultadoPiloto.DescricaoPenalidade,
                        EquipeId = resultadoPiloto.EquipeId,
                        EtapaId = resultadoPiloto.EtapaId,
                        MelhorVolta = resultadoPiloto.MelhorVolta,
                        PilotoId = resultadoPiloto.PilotoId,
                        Pontos = +resultadoPiloto.Pontos,
                        PontosPenalidade = resultadoPiloto.PontosPenalidade,
                        Posicao = resultadoPiloto.Posicao,
                        PosicaoLargada = resultadoPiloto.PosicaoLargada,
                        ResultadoId = resultadoPiloto.ResultadoId,
                        TempoMelhorVolta = resultadoPiloto.TempoMelhorVolta,
                        TempoTotal = resultadoPiloto.TempoTotal,
                        TotalVoltas = resultadoPiloto.TotalVoltas
                    };
                    resultadoOrdenado.Add(result);
                }
            }

            var list = resultadoOrdenado.GroupBy(r => r.PilotoId) // Agrupar resultados por PilotoId
                                   .Select(g => new
                                   {
                                       PilotoId = g.Key,
                                       Pontos = g.Sum(r => r.Pontos),
                                       Posicoes1 = g.Count(r => r.Posicao == 1),
                                       Posicoes2 = g.Count(r => r.Posicao == 2),
                                       Posicoes3 = g.Count(r => r.Posicao == 3),
                                       Posicoes4 = g.Count(r => r.Posicao == 4),
                                       Posicoes5 = g.Count(r => r.Posicao == 5),
                                       Posicoes6 = g.Count(r => r.Posicao == 6),
                                       Posicoes7 = g.Count(r => r.Posicao == 7),
                                       Posicoes8 = g.Count(r => r.Posicao == 8),
                                       Posicoes9 = g.Count(r => r.Posicao == 9),
                                       Posicoes10 = g.Count(r => r.Posicao == 10),
                                       Posicoes11 = g.Count(r => r.Posicao == 11),
                                       Posicoes12 = g.Count(r => r.Posicao == 12),
                                   }) // Obter somas dos pontos e contagens de posições para cada piloto
                                   .OrderByDescending(p => p.Pontos) // Ordenar por pontos do maior para o menor
                                   .ThenByDescending(p => p.Posicoes1) // Em caso de empate nos pontos, priorizar mais posições igual a um
                                   .ThenByDescending(p => p.Posicoes2) // Em caso de empate nos pontos e posições igual a um, priorizar mais posições igual a dois
                                   .ThenByDescending(p => p.Posicoes3) // Em caso de empate nos pontos, posições igual a um e posições igual a dois, priorizar mais posições igual a três
                                   .ThenByDescending(p => p.Posicoes4)
                                   .ThenByDescending(p => p.Posicoes5)
                                   .ThenByDescending(p => p.Posicoes6)
                                   .ThenByDescending(p => p.Posicoes7)
                                   .ThenByDescending(p => p.Posicoes8)
                                   .ThenByDescending(p => p.Posicoes9)
                                   .ThenByDescending(p => p.Posicoes10)
                                   .ThenByDescending(p => p.Posicoes11)
                                   .ThenByDescending(p => p.Posicoes12)
                                   .ToList();

            
            listaResultadoEtapasPorPiloto = listaResultadoEtapasPorPiloto.OrderBy(x => list.FirstOrDefault(y => y.PilotoId == x.PilotoId)?.Pontos).ToList();

                        
            //for (int i = 0; i < listaResultadoEtapasPorPiloto.Count(); i++)
            //{
            //    listaResultadoEtapasPorPiloto[i].Posicao = list.FirstOrDefault(x => x.PilotoId == listaResultadoEtapasPorPiloto[i].PilotoId).;
            //}
            return listaResultadoEtapasPorPiloto;
        }

        private List<ResultadoTemporada> CalcularResultadoTemporadaF1(Temporada temporada)
        {
            throw new NotImplementedException();
        }

        private bool CalcularResultadoEtapaAkgp(List<ResultadoCorrida> resultadoCorridas)
        {
            var existeMelhorVolta = resultadoCorridas.Any(x => x.MelhorVolta == true);
            if (existeMelhorVolta)
            {
                var regrasAkgp = new RegrasAkgp();
                var listaPontuação = regrasAkgp.MontarListaPontos();
                
                //Zera a pontução do piloto ausente
                foreach (var item in resultadoCorridas)
                {
                    if (item.Posicao == 0)
                    {
                        item.Pontos = 0;
                    }
                    if (item.Posicao < 0)
                    {
                        item.Posicao = item.Posicao * (-1);
                    }
                }
                //Cria uma lista somente com os pilotos presentes
                var ResultadoPilotosPresentes = resultadoCorridas.Where(x=>x.Posicao !=0).ToList();
                //Ordena os resultados por posição para poder distribuir os pontos
                resultadoCorridas = ResultadoPilotosPresentes.OrderBy(x => x.Posicao).ToList();
                //Verifica se é a ultima etapa.
                if (_etapaService.VerificaSeUltimaEtapa(resultadoCorridas[0].EtapaId))
                {
                    for (int i = 0; i < resultadoCorridas.Count(); i++)
                    {
                        //⦁	A última etapa do campeonato terá a sua pontuação dobrada,
                        //com exceção da pontuação de melhor volta.
                        //Sendo assim o 1º colocado receberá 100 pontos, o 2º colocado receberá 80 pontos, o 3º colocado receberá 70 pontos e assim sucessivamente.
                        resultadoCorridas[i].Pontos = ((listaPontuação[i] * 2) - resultadoCorridas[i].PontosPenalidade);
                        if (resultadoCorridas[i].MelhorVolta == true)
                        {
                            resultadoCorridas[i].Pontos += 2;
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < resultadoCorridas.Count(); i++)
                    {
                        resultadoCorridas[i].Pontos = (listaPontuação[i] - resultadoCorridas[i].PontosPenalidade);
                        if (resultadoCorridas[i].MelhorVolta == true)
                        {
                            resultadoCorridas[i].Pontos += 2;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        private bool CalcularResultadoEtapaF1(List<ResultadoCorrida> resultadoCorridas)
        {
            return false;
        }

    }
}
