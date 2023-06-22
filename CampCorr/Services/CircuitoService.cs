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
        public async Task<Circuito> BuscarCircuitoAsync(int circuitoId)
        {
            return await _circuitoRepository.BuscarCircuitoAsync(circuitoId);
        }
    }
}
