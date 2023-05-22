using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        public void Salvar(Etapa etapa)
        {
            _context.Add(etapa);
            _context.SaveChanges();
        }
        public void Atualizar(Etapa etapa)
        {
            _context.Update(etapa);
            _context.SaveChanges();
        }
        public void Remover(Etapa etapa)
        {
            _context.Remove(etapa);
            _context.SaveChanges();
        }
        public async Task<Etapa> BuscarEtapaAsync(string nomeUsuario, string numeroEtapa, int ano)
        {
            var temporadaId = await _temporadaRepository.BuscarIdTemporadaPorNomeUsuarioAsync(nomeUsuario, ano);
            return await _context.Etapas
                .Where(x => x.TemporadaId == temporadaId
                    && x.NumeroEvento.Substring(0, 1) == numeroEtapa.ToString())
                .FirstOrDefaultAsync();
        }
        public async Task<Etapa> BuscarEtapaAsync(int etapaId)
        {
            return await _context.Etapas.Where(x => x.EtapaId == etapaId).FirstOrDefaultAsync();
        }
        public Etapa BuscarEtapa(int etapaId)
        {
            return _context.Etapas.Where(x => x.EtapaId == etapaId).FirstOrDefault();
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

        public List<Etapa> ListarEtapasTemporada(int temporadaId)
        {
            return _context.Etapas.Where(x => x.TemporadaId == temporadaId).ToList();
        }

        public Circuito BuscarCircuito(int CircuitoId)
        {
            return _context.Circuitos.Where(x => x.CircuitoId == CircuitoId).FirstOrDefault();
        }

        
        //public bool ValidarEtapa(int etapaId, DateTime data)
        //{
        //    var etapa = _context.Etapas.Where(x => x.EtapaId == etapaId && x.Data == data).FirstOrDefault();
        //    var temporada = _context.Temporadas.Where(x => x.TemporadaId == etapa.TemporadaId).FirstOrDefault();
        //    var campeonatoId = _context.Campeonatos.Where(x => x.CampeonatoId == temporada.CampeonatoId).Select(x=>x.CampeonatoId).FirstOrDefault();
        //    //verifica se conseguiu encontrar uma etapa com o id e a data e verifica se essa etapa pertence a esse campeonato
        //    if (etapa == null || _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuarioAsync(nomeUsuario) != campeonatoId)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        public void ConcluirEtapa(int etapaId)
        {
            var etapa = _context.Etapas.Where(x => x.EtapaId == etapaId).FirstOrDefault();
            etapa.Concluido = true;
            _context.Update(etapa);
            _context.SaveChanges();
        }
        //Método para buscar o número da etapa atual e não o Id
        public int BuscarNumeroEtapaAtual(int temporadaId)
        {
            return _context.Etapas.Where(x => x.TemporadaId == temporadaId).Count() + 1;
        }
        public int QuantidadeEtapas(int temporadaId)
        {
            return _context.Temporadas.Where(x => x.TemporadaId == temporadaId).FirstOrDefault().QuantidadeEtapas;
        }
    }
}
