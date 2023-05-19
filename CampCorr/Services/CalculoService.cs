using CampCorr.Models;
using CampCorr.Repositories;
using CampCorr.Services.Interfaces;
using CampCorr.Negocios;
using CampCorr.Context;
using CampCorr.Repositories.Interfaces;

namespace CampCorr.Services
{
    public class CalculoService : ICalculoService
    {

        private readonly IRegulamentoRepository _regulamentoRepository;
        private readonly IEtapaRepository _etapaRepository;

        public CalculoService(IRegulamentoRepository regulamentoRepository, IEtapaRepository etapaRepository)
        {
            _regulamentoRepository = regulamentoRepository;
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
