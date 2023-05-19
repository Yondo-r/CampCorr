using CampCorr.Models;

namespace CampCorr.Services.Interfaces
{
    public interface ICircuitoService
    {
        List<Circuito> ListarCircuitos();
        Task<Circuito> BuscarCircuitoAsync(int circuitoId);
    }
}
