using CampCorr.Models;

namespace CampCorr.Services.Interfaces
{
    public interface IRegulamentoService
    {
        Regulamento BuscarRegulamento(int regulamentoId);
        Regulamento BuscarRegulamentoPorEtapa(int etapaId);
    }
}
