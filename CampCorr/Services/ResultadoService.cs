using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;
using CampCorr.ViewModels;

namespace CampCorr.Services
{
    public class ResultadoService : IResultadoService
    {
        private readonly IResultadoRepository _resultadoRepository;
        private readonly ITemporadaRepository _temporadaRepository;

        public ResultadoService(IResultadoRepository resultadoRepository, ITemporadaRepository temporadaRepository)
        {
            _resultadoRepository = resultadoRepository;
            _temporadaRepository = temporadaRepository;
        }
        public void Salvar(ResultadoCorrida resultadoCorrida)
        {
            _resultadoRepository.SalvarResultadoCorrida(resultadoCorrida);
        }
        public async Task<int> BuscarResultadoIdAsync(int etapaId, int pilotoId)
        {
            return await _resultadoRepository.BuscarResultadoIdAsync(etapaId, pilotoId);
        }
        public ResultadoCorridaViewModel BuscarPilotoResultadoEtapa(int etapaId, int pilotoId)
        {
            return _resultadoRepository.BuscarPilotoResultadoEtapa(etapaId, pilotoId);
        }
        public async Task<List<ResultadoCorrida>> ListarResultadoEtapa(int etapaId)
        {
            return await _resultadoRepository.ListarResultadoEtapa(etapaId);
        }
        public List<ResultadoCorrida> MontaListaResultadoTemporada(int temporadaId)
        {
            return _resultadoRepository.MontaListaResultadoTemporada(temporadaId);
        }
        public List<ResultadoTemporada> MontaListaResultadoFinalTemporada(int temporadaId)
        {
            return _resultadoRepository.MontaListaResultadoFinalTemporada(temporadaId);
        }
        public void ConcluirTemporada(List<ResultadoTemporada> resultadosTemporada, Temporada temporada)
        {
            _resultadoRepository.SalvarResultadosTemporada(resultadosTemporada);
            temporada.Concluida = true;
            _temporadaRepository.Atualizar(temporada);
        }
    }
}
