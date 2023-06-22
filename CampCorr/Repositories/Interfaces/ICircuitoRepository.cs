using CampCorr.Models;

namespace CampCorr.Repositories.Interfaces
{
    public interface ICircuitoRepository
    {
        void Add(Circuito circuito);
        List<Circuito> ListarCircuitos();
        Task<Circuito> BuscarCircuitoAsync(int circuitoId);
    }
}
