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
        public void Salvar(Temporada temporada)
        {
            _context.Add(temporada);
            _context.SaveChanges();
        }
        public void Atualizar(Temporada temporada)
        {
            _context.Update(temporada);
            _context.SaveChanges();
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
        public Temporada BuscarTemporada(int temporadaId)
        {
            return _context.Temporadas.FirstOrDefault(x => x.TemporadaId == temporadaId);
        }
        public bool TemporadaExists(int id)
        {
            return _context.Temporadas.Any(e => e.TemporadaId == id);
        }
    }
}
