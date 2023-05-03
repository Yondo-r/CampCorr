using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.ViewModels;

namespace CampCorr.Repositories
{
    public class CampeonatoRepository : ICampeonatoRepository
    {
        private readonly AppDbContext _context;

        public CampeonatoRepository(AppDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<Campeonato> Campeonatos => _context.Campeonatos;
        
        public void Salvar(Campeonato campeonato)
        {
            _context.Add(campeonato);
            _context.SaveChangesAsync();
        }

        public void RemovePilotoCampeonato(int idPiloto, int idCampeonato)
        {
            var pilotoCampeonato = _context.PilotosCampeonatos.Where(x => x.PilotoId == idPiloto && x.CampeonatoId == idCampeonato).FirstOrDefault();
            _context.Remove(pilotoCampeonato);
            _context.SaveChanges();
        }
        public string BuscarIdCampeonatoPorIdUsuario(string userId)
        {
            return _context.Campeonatos.FirstOrDefault(x => x.UserId == userId).CampeonatoId.ToString();
        }
        public int BuscarIdCampeonatoPorNomeUsuario(string nomeUsuario)
        {
            var userId = _context.Users.Where(x => x.UserName.Contains(nomeUsuario)).FirstOrDefault().Id;
            return _context.Campeonatos.FirstOrDefault(x => x.UserId == userId).CampeonatoId;
        }
    }
}
