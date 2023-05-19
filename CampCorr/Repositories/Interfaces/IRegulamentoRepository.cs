using CampCorr.Models;

namespace CampCorr.Repositories.Interfaces
{
    public interface IRegulamentoRepository
    {
        Regulamento BuscarRegulamentoPorEtapaId(int etapaId);
        Regulamento BuscarRegulamento(int RegulamentoId);
    }
}
