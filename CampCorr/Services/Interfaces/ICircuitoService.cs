using CampCorr.Models;

namespace CampCorr.Services.Interfaces
{
    public interface ICircuitoService
    {
        void Add(Circuito circuito);
        List<Circuito> ListarCircuitos();
        Task<Circuito> BuscarCircuitoAsync(int circuitoId);
    }
}
