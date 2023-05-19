using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampCorr.Repositories
{
    public class TemporadaRepository : ITemporadaRepository
    {
        private readonly AppDbContext _context;
        private readonly ICampeonatoRepository _campeonatoRepository;

        public TemporadaRepository(AppDbContext context, ICampeonatoRepository campeonatoRepository)
        {
            _context = context;
            _campeonatoRepository = campeonatoRepository;
        }
        public async void Salvar(Temporada temporada)
        {
            _context.Add(temporada);
            await _context.SaveChangesAsync();
        }
        public async void Atualizar(Temporada temporada)
        {
            _context.Update(temporada);
            await _context.SaveChangesAsync();
        }
        public async Task<int> BuscarIdTemporadaPorNomeUsuarioAsync(string nomeUsuario, int anoTemporada)
        {
            var campeonatoId = await _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuarioAsync(nomeUsuario);
            var temporada = await _context.Temporadas.Where(x => x.CampeonatoId == campeonatoId && x.AnoTemporada == anoTemporada).FirstOrDefaultAsync();
            return temporada.TemporadaId;
        }
        public List<Temporada> ListarTemporadasDoCampeonato(int campeonatoId)
        {
            return  _context.Temporadas.Where(x => x.CampeonatoId == campeonatoId).ToList();
        }
        public async Task<Temporada> BuscarTemporadaAsync(int campeonatoId, int anoTemporada)
        {
            return await _context.Temporadas.Where(x => x.CampeonatoId == campeonatoId && x.AnoTemporada == anoTemporada).FirstOrDefaultAsync();
        }
        public async Task<Temporada> BuscarTemporadaAsync(int tempodaraId)
        {
            return await _context.Temporadas.Where(x => x.TemporadaId == tempodaraId).FirstOrDefaultAsync();
        }
        public bool TemporadaExists(int id)
        {
            return _context.Temporadas.Any(e => e.TemporadaId == id);
        }
    }
}
