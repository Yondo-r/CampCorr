using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CampCorr.Repositories
{
    public class PilotoRepository : IPilotoRepository
    {
        
        private readonly AppDbContext _context;
        //private readonly string nomeUsuario;

        public PilotoRepository(AppDbContext context)
        {
            _context = context;
        }
        public void SalvarPilotoCampeonato(PilotoCampeonato pilotoCampeonato)
        {
            _context.Add(pilotoCampeonato);
            _context.SaveChanges();
        }
        public void RemoverPilotoCampeonato(int pilotoId, int campeonatoId)
        {
            var pilotoCampeonato = _context.PilotosCampeonatos.Where(x => x.PilotoId == pilotoId && x.CampeonatoId == campeonatoId).FirstOrDefault();
            _context.Remove(pilotoCampeonato);
            _context.SaveChanges();
        }
        public void SalvarPilotoEquipe(PilotoEquipe pilotoEquipe)
        {
            _context.Add(pilotoEquipe);
            _context.SaveChanges();
        }
        public void RemoverPilotoEquipe(PilotoEquipe pilotoEquipe)
        {
            _context.Remove(pilotoEquipe);
             _context.SaveChanges();
        }
        public void SalvarPilotoTemporada(PilotoTemporada pilotoTemporada)
        {
            _context.Add(pilotoTemporada);
            _context.SaveChanges();
        }
        public void RemoverPilotoTemporada(PilotoTemporada pilotoTemporada)
        {
            _context.Remove(pilotoTemporada);
            _context.SaveChanges();
        }

        public List<Piloto> ListarPilotos()
        {
            return _context.Pilotos.ToList();
        }
        public List<PilotoCampeonato> ListarPilotosCampeonato()
        {
            return _context.PilotosCampeonatos.ToList();
        }
        //Monta uma VM de piloto com campeonato
        public List<PilotoViewModel> ListarPilotoVmCampeonato(int campeonatoId)
        {
            List<PilotoViewModel> pilotosCampeonato = new List<PilotoViewModel>();
            var idsPilotosCampeonato = _context.Pilotos.Join(_context.PilotosCampeonatos,
                p => p.PilotoId,
                pc => pc.PilotoId, (p, pc) => new { p, pc })
                .Join(_context.Campeonatos,
                x => x.pc.CampeonatoId,
                c => c.CampeonatoId, (p1, c) => new { p1, c })
                .Where(y => y.c.CampeonatoId == campeonatoId).Select(x => x.p1.p.UsuarioId).ToList();
            foreach (var userId in idsPilotosCampeonato)
            {
                PilotoViewModel pilotoVm = new PilotoViewModel() { };
                var piloto = _context.Pilotos.Where(x => x.UsuarioId == userId).FirstOrDefault();
                pilotoVm.NomePiloto = piloto.Nome;
                pilotoVm.PilotoId = piloto.PilotoId;
                pilotoVm.UserLogin = _context.Users.Where(x => x.Id == userId).Select(x => x.UserName).FirstOrDefault();
                pilotosCampeonato.Add(pilotoVm);
            }
            return pilotosCampeonato;
        }
        public List<Piloto> BuscarPilotosCadastradosCampeonato(int campeonatoId)
        {
            List<Piloto> pilotosCampeonato = new List<Piloto>(); 
                var idPilotosCampeonato = _context.Pilotos.Join(_context.PilotosCampeonatos,
                p => p.PilotoId,
                pc => pc.PilotoId, (p, pc) => new { p, pc })
                .Join(_context.Campeonatos,
                cp => cp.pc.CampeonatoId,
                c => c.CampeonatoId, (cp, c) => new { cp, c })
                .Where(x => x.c.CampeonatoId == campeonatoId).Select(x => x.cp.p.PilotoId)
                .ToList();
            foreach (var item in idPilotosCampeonato)
            {
                pilotosCampeonato.Add(_context.Pilotos.Where(x => x.PilotoId == item).FirstOrDefault());
            }
            return pilotosCampeonato;
        }

        public List<PilotoViewModel> PreencherListaDePilotosNaoAdicionadosTemporada(List<PilotoViewModel> listaPilotoCampeonatoVM, List<PilotoViewModel> listaPilotosTemporadaAdicionado)
        {
            List<PilotoViewModel> listaPilotoNaoAdicionadoTemporada = new List<PilotoViewModel>();
            foreach (var piloto in listaPilotoCampeonatoVM)
            {
                if (!listaPilotosTemporadaAdicionado.Select(x => x.PilotoId).Contains(piloto.PilotoId))
                {
                    listaPilotoNaoAdicionadoTemporada.Add(piloto);
                }
            }
            return listaPilotoNaoAdicionadoTemporada;
        }

        public List<PilotoViewModel> PreencherListaDePilotosTemporada(int temporadaId)
        {
            List<PilotoViewModel> listaPilotosAdicionados = new List<PilotoViewModel>();
            var listaIdPilotosAdicionados = _context.Pilotos.Join(_context.PilotosTemporadas,
                p => p.PilotoId,
                pt => pt.PilotoId, (p, pt) => new { p, pt })
                .Join(_context.Temporadas,
                tp => tp.pt.TemporadaId,
                t => t.TemporadaId, (tp, t) => new { tp, t })
                .Where(x => x.t.TemporadaId == temporadaId).Select(x => x.tp.p.PilotoId)
                .ToList();
            foreach (var id in listaIdPilotosAdicionados)
            {
                PilotoViewModel pilotoVM = new PilotoViewModel()
                {
                    NomePiloto = _context.Pilotos.Where(x => x.PilotoId == id).Select(x => x.Nome).FirstOrDefault(),
                    PilotoId = id,
                };
                listaPilotosAdicionados.Add(pilotoVM);
            }
            return listaPilotosAdicionados;
        }

        public List<ResultadoCorridaViewModel> PreencherListaDePilotosTemporadaComEquipe(int temporadaId)
        {
            List<ResultadoCorridaViewModel> listaPilotosAdicionados = new List<ResultadoCorridaViewModel>();
            var listaIdPilotosAdicionados = _context.PilotosEquipes.Join(_context.Equipes,
                pe => pe.EquipeId,
                e => e.EquipeId, (pe, e) => new { pe, e })
                .Join(_context.EquipeTemporadas,
                x => x.e.EquipeId,
                et => et.EquipeId, (x, et) => new { x, et })
                .Where(x => x.et.TemporadaId == temporadaId).ToList();
            foreach (var item in listaIdPilotosAdicionados)
            {
                ResultadoCorridaViewModel pilotoVM = new ResultadoCorridaViewModel()
                {
                    NomePiloto = _context.Pilotos.Where(x => x.PilotoId == item.x.pe.PilotoId).Select(x => x.Nome).FirstOrDefault(),
                    EquipeId = item.x.pe.EquipeId,
                    PilotoId = item.x.pe.PilotoId
                };
                listaPilotosAdicionados.Add(pilotoVM);
            }
            return listaPilotosAdicionados;
        }

        public int BuscarIdPilotoComUsuario(string idUsuario)
        {
            return _context.Pilotos.Where(x => x.UsuarioId == idUsuario).FirstOrDefault().PilotoId;
        }

        public Piloto BuscarPilotoPorId(int pilotoId)
        {
            return _context.Pilotos.Where(x => x.PilotoId.Equals(pilotoId)).FirstOrDefault();
        }
        public async Task<PilotoEquipe> BuscarPilotoEquipeAsync(int pilotoId, int equipeId)
        {
            return await _context.PilotosEquipes.Where(x => x.PilotoId == pilotoId && x.EquipeId == equipeId).FirstOrDefaultAsync();
        }
        public async Task<PilotoTemporada> BuscarPilotoTemporadaAsync(int pilotoId, int temporadaId)
        {
            return await _context.PilotosTemporadas.Where(x => x.PilotoId == pilotoId && x.TemporadaId == temporadaId).FirstOrDefaultAsync();
        }
        public Piloto BuscarPiloto(int pilotoId)
        {
            return _context.Pilotos.Where(x => x.PilotoId == pilotoId).FirstOrDefault();
        }

        public List<Piloto> MontaListaPilotosTemporada(int temporadaId)
        {
            List<Piloto> listaPilotos = new List<Piloto>();
            var idsPilotos = _context.ResultadosCorrida.Join(_context.Etapas,
                rc => rc.EtapaId,
                et => et.EtapaId, (rc, et) => new { rc, et })
                .Join(_context.Temporadas,
                Tp => Tp.et.TemporadaId,
                T => T.TemporadaId, (Tp, T) => new { Tp, T })
                .Where(x => x.T.TemporadaId == temporadaId).Select(x => x.Tp.rc.PilotoId).Distinct().ToList();

            foreach (int id in idsPilotos)
            {
                listaPilotos.Add(_context.Pilotos.Where(x => x.PilotoId == id).FirstOrDefault());
            }

            return listaPilotos;
        }
    }
}
