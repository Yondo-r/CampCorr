using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CampCorr.Repositories
{
    public class PilotoRepository : IPilotoRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;
        //private readonly string nomeUsuario;

        public PilotoRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
            var listaIdPilotosAdicionados = _context.PilotosTemporadas.Join(_context.PilotosEquipes,
                pt => pt.PilotoId,
                pe => pe.PilotoId, (pt, pe) => new { pt, pe })
                .Where(x => x.pt.TemporadaId == temporadaId)
                .ToList();
            foreach (var item in listaIdPilotosAdicionados)
            {
                ResultadoCorridaViewModel pilotoVM = new ResultadoCorridaViewModel()
                {
                    NomePiloto = _context.Pilotos.Where(x => x.PilotoId == item.pt.PilotoId).Select(x => x.Nome).FirstOrDefault(),
                    EquipeId = item.pe.EquipeId,
                    PilotoId = item.pe.PilotoId
                };
                listaPilotosAdicionados.Add(pilotoVM);
            }
            return listaPilotosAdicionados;
        }

        public List<PilotoViewModel> PreencherListaDePilotosTemporadaSemEquipe(int temporadaId)
        {
            List<PilotoViewModel> listaPilotosTemporada = PreencherListaDePilotosTemporada(temporadaId);
            var resultadoVm = PreencherListaDePilotosTemporada(temporadaId);
            
            var listaPilotoComEquipe = PreencherListaDePilotosTemporadaComEquipe(temporadaId);
            
            foreach (var piloto in listaPilotoComEquipe)
            {
                if (listaPilotosTemporada.Select(x => x.PilotoId).Contains(piloto.PilotoId))
                {
                    listaPilotosTemporada.Remove(listaPilotosTemporada.Where(x=>x.PilotoId == piloto.PilotoId).FirstOrDefault());
                }
            }
            return listaPilotosTemporada;
        }

        public int BuscarIdPilotoComUsuario(string idUsuario)
        {
            return _context.Pilotos.Where(x => x.UsuarioId == idUsuario).FirstOrDefault().PilotoId;
        }

        public Piloto BuscarPilotoPorId(int pilotoId)
        {
            return _context.Pilotos.Where(x => x.PilotoId.Equals(pilotoId)).FirstOrDefault();
        }
    }
}
