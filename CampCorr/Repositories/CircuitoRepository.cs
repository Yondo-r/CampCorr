using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampCorr.Repositories
{
    public class CircuitoRepository : ICircuitoRepository
    {
        private readonly AppDbContext _context;

        public CircuitoRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Circuito circuito)
        {
            _context.Add(circuito);
            _context.SaveChanges();
        }
        public List<Circuito> ListarCircuitos()
        {
            return _context.Circuitos.ToList();
        }
        public async Task<Circuito> BuscarCircuitoAsync(int circuitoId)
        {
            return await _context.Circuitos.Where(x => x.CircuitoId == circuitoId).FirstOrDefaultAsync();
        }
    }
}
