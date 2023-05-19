using CampCorr.Models;
using CampCorr.ViewModels;

namespace CampCorr.Repositories.Interfaces
{
    public interface IResultadoRepository
    {
        ResultadoCorridaViewModel BuscarPilotoResultadoEtapa(int etapaId, int pilotoId);
        public void SalvarResultado(ResultadoCorrida resultado);
        Task<int> BuscarResultadoIdAsync(int etapaId, int pilotoId);
        Task<List<ResultadoCorrida>> ListarResultadoEtapa(int etapaId);
        List<ResultadoCorrida> MontaListaResultadoTemporada(int temporadaId);
    }
}
