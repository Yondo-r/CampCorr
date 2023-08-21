using CampCorr.Models;

namespace CampCorr.Services.Interfaces
{
    public interface ICircuitoService
    {
        void Add(Circuito circuito);
        List<Circuito> ListarCircuitos();
        List<string> ListarTipos();
        Task<List<Circuito>> ListarCircuitosAsync(string tipo);
        Task<Circuito> BuscarCircuitoAsync(int circuitoId);
    }
}
