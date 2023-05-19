using CampCorr.Models;

namespace CampCorr.Repositories.Interfaces
{
    public interface ICircuitoRepository
    {
        List<Circuito> ListarCircuitos();
        Task<Circuito> BuscarCircuitoAsync(int circuitoId);
    }
}
