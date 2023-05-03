using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampCorr.Repositories
{
    public class EtapaRepository : IEtapaRepository
    {
        private readonly AppDbContext _context;
        private readonly ITemporadaRepository _temporadaRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICampeonatoRepository _campeonatoRepository;
        private readonly string nomeUsuario;

        public EtapaRepository(AppDbContext context, ITemporadaRepository temporadaRepository, SignInManager<IdentityUser> signInManager, ICampeonatoRepository campeonatoRepository)
        {
            _context = context;
            _temporadaRepository = temporadaRepository;
            _signInManager = signInManager;
            _campeonatoRepository = campeonatoRepository;
            nomeUsuario = _signInManager.Context.User.Identity.Name; 
        }

        public int BuscarIdEtapaPorNomeUsuario(string nomeUsuario, string numeroEtapa, int anoTemporada)
        {
            return _context.Etapas.Where
                    (x => x.TemporadaId == _temporadaRepository.BuscarIdTemporadaPorNomeUsuario(nomeUsuario, anoTemporada)
                    && x.NumeroEvento.Substring(0, 1) == numeroEtapa.ToString()).FirstOrDefault().EtapaId;
        }
        //Função para buscar todas as etapas de todas as temporadas de um campeonato
        public List<Etapa> BuscarListaEtapasCampeonato(int campeonatoId)
        {
            List<Etapa> listaEtapa = new List<Etapa>();
            var listaIdEtapas = _context.Etapas.Join(_context.Temporadas,
                etapa => etapa.TemporadaId,
                t => t.TemporadaId, (etapa, t) => new { etapa, t})
                .Join(_context.Campeonatos,
                x => x.t.CampeonatoId,
                c => c.CampeonatoId, (e1, t) => new {e1, t})
                .Where(y => y.t.CampeonatoId == campeonatoId).Select(x => x.e1.etapa.EtapaId)
                .ToList();
            foreach (var id in listaIdEtapas)
            {
                listaEtapa.Add(_context.Etapas.Where(x => x.EtapaId == id).FirstOrDefault());
            }
            return listaEtapa.OrderBy(x=>x.Data).ToList();
        }

        public Circuito BuscarKartodromo(int kartodromoId)
        {
            return _context.Circuitos.Where(x => x.CircuitoId == kartodromoId).FirstOrDefault();
        }

        public Etapa BuscarEtapaPorId(int etapaId)
        {
            return _context.Etapas.Where(x => x.EtapaId == etapaId).FirstOrDefault();
        }
        public bool ValidarEtapa(int etapaId, DateTime data)
        {
            var etapa = _context.Etapas.Where(x => x.EtapaId == etapaId && x.Data == data).FirstOrDefault();
            var temporada = _context.Temporadas.Where(x => x.TemporadaId == etapa.TemporadaId).FirstOrDefault();
            var campeonatoId = _context.Campeonatos.Where(x => x.CampeonatoId == temporada.CampeonatoId).Select(x=>x.CampeonatoId).FirstOrDefault();
            //verifica se conseguiu encontrar uma etapa com o id e a data e verifica se essa etapa pertence a esse campeonato
            if (etapa == null || _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(nomeUsuario) != campeonatoId)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
