using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.ViewModels;
using Microsoft.EntityFrameworkCore;

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
            _context.SaveChanges();
        }
        public void Atualizar(Campeonato campeonato)
        {
            _context.Update(campeonato);
            _context.SaveChanges();
        }
        public List<Campeonato> ListarCampeonatos()
        {
            return _context.Campeonatos.ToList();
        }
        
        public string BuscarIdCampeonatoPorIdUsuario(string userId)
        {
            return _context.Campeonatos.FirstOrDefault(x => x.UserId == userId).CampeonatoId.ToString();
        }
        public async Task<int> BuscarIdCampeonatoPorNomeUsuarioAsync(string nomeUsuario)
        {
            var userId = _context.Users.Where(x => x.UserName.Contains(nomeUsuario)).FirstOrDefault().Id;
            var campeonato = await _context.Campeonatos.FirstOrDefaultAsync(x => x.UserId == userId);
            return campeonato.CampeonatoId;
        }
        public int BuscarIdCampeonatoPorNomeUsuario(string nomeUsuario)
        {
            var userId = _context.Users.Where(x => x.UserName.Contains(nomeUsuario)).FirstOrDefault().Id;
            var campeonato = _context.Campeonatos.FirstOrDefault(x => x.UserId == userId);
            return campeonato.CampeonatoId;
        }
        public async Task<Campeonato> BuscarCampeonatoPorId(int campeonatoId)
        {
            return await _context.Campeonatos.FindAsync(campeonatoId);
        }
    }
}
