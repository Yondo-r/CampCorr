using CampCorr.Context;
using CampCorr.Repositories.Interfaces;

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

        public int BuscarIdTemporadaPorNomeUsuario(string nomeUsuario, int anoTemporada)
        {
            return _context.Temporadas.Where(x => x.CampeonatoId == _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(nomeUsuario) && x.AnoTemporada == anoTemporada).FirstOrDefault().TemporadaId;
        }
    }
}
