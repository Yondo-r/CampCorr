using CampCorr.Models;
using CampCorr.ViewModels;

namespace CampCorr.Repositories.Interfaces
{
    public interface IResultadoRepository
    {
        ResultadoCorridaViewModel BuscarPilotoResultadoEtapa(int etapaId, int pilotoId);
        public void SalvarResultadoCorrida(ResultadoCorrida resultado);
        Task<int> BuscarResultadoIdAsync(int etapaId, int pilotoId);
        Task<List<ResultadoCorrida>> ListarResultadoEtapa(int etapaId);
        List<ResultadoCorrida> ListarResultadoEtapa(int temporadaId, int pilotoId);
        List<ResultadoCorrida> MontaListaResultadoTemporada(int temporadaId);
        List<ResultadoTemporada> MontaListaResultadoFinalTemporada(int temporadaId);
        void SalvarResultadosTemporada(List<ResultadoTemporada> resultadosTemporada);
    }
}
