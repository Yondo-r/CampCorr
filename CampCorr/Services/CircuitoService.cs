using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;

namespace CampCorr.Services
{
    public class CircuitoService : ICircuitoService
    {
        private readonly ICircuitoRepository _circuitoRepository;

        public CircuitoService(ICircuitoRepository circuitoRepository)
        {
            _circuitoRepository = circuitoRepository;
        }

        public void Add(Circuito circuito)
        {
            _circuitoRepository.Add(circuito);
        }
        public List<Circuito> ListarCircuitos()
        {
            return _circuitoRepository.ListarCircuitos();
        }
        public async Task<List<Circuito>> ListarCircuitosAsync(string tipo)
        {
            return await _circuitoRepository.ListarCircuitosAsync(tipo);
        }
        public List<string> ListarTipos()
        {
            return _circuitoRepository.ListarTipos();
        }
        public async Task<Circuito> BuscarCircuitoAsync(int circuitoId)
        {
            return await _circuitoRepository.BuscarCircuitoAsync(circuitoId);
        }
    }
}
