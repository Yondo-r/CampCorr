using CampCorr.Models;
using CampCorr.ViewModels;

namespace CampCorr.Services.Interfaces
{
    public interface IResultadoService
    {
        void Salvar(ResultadoCorrida resultadoCorrida);
        Task<int> BuscarResultadoIdAsync(int EtapaId, int PilotoId);
        Task<List<ResultadoCorrida>> ListarResultadoEtapa(int etapaId);
        List<ResultadoCorrida> MontaListaResultadoTemporada(int temporadaId);
        ResultadoCorridaViewModel BuscarPilotoResultadoEtapa(int etapaId, int pilotoId);
    }
}
