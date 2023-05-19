using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;
using CampCorr.ViewModels;

namespace CampCorr.Services
{
    public class ResultadoService : IResultadoService
    {
        private readonly IResultadoRepository _resultadoRepository;

        public ResultadoService(IResultadoRepository resultadoRepository)
        {
            _resultadoRepository = resultadoRepository;
        }
        public void Salvar(ResultadoCorrida resultadoCorrida)
        {
            _resultadoRepository.SalvarResultado(resultadoCorrida);
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
    }
}
