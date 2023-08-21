using CampCorr.Models;

namespace CampCorr.Repositories.Interfaces
{
    public interface ICircuitoRepository
    {
        void Add(Circuito circuito);
        List<Circuito> ListarCircuitos();
        Task<List<Circuito>> ListarCircuitosAsync(string tipo);
        List<string> ListarTipos();
        Task<Circuito> BuscarCircuitoAsync(int circuitoId);
    }
}
