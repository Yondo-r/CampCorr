using CampCorr.Models;
using CampCorr.Repositories;
using CampCorr.Services.Interfaces;
using CampCorr.Negocios;
using CampCorr.Context;

namespace CampCorr.Services
{
    public class Calculo : ICalculo
    {
        private readonly RegulamentoRepository _regulamentoRepository;
        private readonly ResultadoRepository _resultadoRepository;
        private readonly EtapaRepository _etapaRepository;
        public Calculo(RegulamentoRepository regulamentoRepository, ResultadoRepository resultadoRepository, EtapaRepository etapaRepository)
        {
            _regulamentoRepository = regulamentoRepository;
            _resultadoRepository = resultadoRepository;
            _etapaRepository = etapaRepository;
        }

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
            var existeMelhorVolta = resultadoCorridas.Any(x => x.MelhorVolta == true);
            if (existeMelhorVolta)
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

    }
}
