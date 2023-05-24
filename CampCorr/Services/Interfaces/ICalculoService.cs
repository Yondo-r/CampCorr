using CampCorr.Models;

namespace CampCorr.Services.Interfaces
{
    public interface ICalculoService
    {
        bool CalcularResultadoEtapa(List<ResultadoCorrida> listaResultadoCorridas);
        List<ResultadoTemporada> CalcularResultadoTemporada(int temporadaId);
    }
}