using CampCorr.Models;

namespace CampCorr.Repositories.Interfaces
{
    public interface IRegulamentoRepository
    {
        public Regulamento BuscarRegulamentoPorEtapaId(int etapaId);
    }
}
