using CampCorr.ViewModels;

namespace CampCorr.Repositories.Interfaces
{
    public interface IResultadoRepository
    {
        ResultadoCorridaViewModel BuscarPilotoResultadoEtapa(int etapaId, int pilotoId);
    }
}
