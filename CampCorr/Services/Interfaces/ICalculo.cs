using CampCorr.Models;

namespace CampCorr.Services.Interfaces
{
    public interface ICalculo
    {
        bool CalcularResultadoEtapa(List<ResultadoCorrida> listaResultadoCorridas);
    }
}