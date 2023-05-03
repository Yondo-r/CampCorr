using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampCorr.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Adm")]
    public class EquipesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICampeonatoRepository _campeonatoRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITemporadaRepository _temporadaRepository;
        private readonly string nomeUsuario;
        private readonly IEquipeRepository _equipeRepository;
        private readonly IPilotoRepository _pilotoRepository;

        public EquipesController(AppDbContext context, SignInManager<IdentityUser> signInManager, IUsuarioRepository usuarioRepository, ICampeonatoRepository campeonatoRepository, ITemporadaRepository temporadaRepository, IEquipeRepository equipeRepository, IPilotoRepository pilotoRepository)
        {
            _context = context;
            _signInManager = signInManager;
            _usuarioRepository = usuarioRepository;
            nomeUsuario = signInManager.Context.User.Identity.Name;
            _campeonatoRepository = campeonatoRepository;
            _temporadaRepository = temporadaRepository;
            _equipeRepository = equipeRepository;
            _pilotoRepository = pilotoRepository;
        }
        public IActionResult Create(int anoTemporada)
        {
            TempData["anoTemporada"] = anoTemporada;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int anoTemporada, [Bind("EquipeId,Nome")] Equipe equipe)
        {
            var campeonatoId = _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(_signInManager.Context.User.Identity.Name);
            equipe.CampeonatoId = campeonatoId;
            if (ModelState.IsValid)
            {
                _context.Add(equipe);
                await _context.SaveChangesAsync();
                return RedirectToAction("VisualizarEquipes", new { anoTemporada = anoTemporada });

            }
            return View();
        }
        public async Task<IActionResult> VisualizarEquipes(int anoTemporada)
        {

            TempData["anoTemporada"] = anoTemporada;
            var temporada = await _context.Temporadas.Where(x => x.CampeonatoId == _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(nomeUsuario) && x.AnoTemporada == anoTemporada).FirstOrDefaultAsync();
            var equipesCadastradas = _context.Equipes.Where(x => x.CampeonatoId == _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(nomeUsuario)).ToList();
            var equipesAdicionadas =  _equipeRepository.PreencherListaEquipesAdicionadas(temporada.TemporadaId);
            var equipesNaoAdicionadas = PreencherListaEquipesNaoAdicionadas(equipesAdicionadas, equipesCadastradas);

            ViewBag.equipesCadastradas = equipesCadastradas;
            ViewBag.equipesAdicionadas = equipesAdicionadas;
            ViewBag.equipesNaoAdicionadas = equipesNaoAdicionadas;
            return View(equipesCadastradas);
        }



        [HttpPost]
        public async Task<JsonResult> PostAddEquipes(List<string> idEquipe, int anoTemporada, [Bind("Id,EquipeId,TemporadaId")] EquipeTemporada equipeTemporada)
        {
            var temporadaId = _temporadaRepository.BuscarIdTemporadaPorNomeUsuario(nomeUsuario, anoTemporada);

            if (ModelState.IsValid)
            {
                foreach (var id in idEquipe)
                {
                    equipeTemporada.Id = 0;
                    equipeTemporada.TemporadaId = temporadaId;
                    equipeTemporada.EquipeId = Convert.ToInt32(id);
                    _context.Add(equipeTemporada);
                    await _context.SaveChangesAsync();
                }
            }

            return new JsonResult(Ok());
        }
        [HttpPost]
        public async Task<JsonResult> PostRemoverEquipes(List<string> idEquipe, int anoTemporada)
        {
            var temporadaId = _temporadaRepository.BuscarIdTemporadaPorNomeUsuario(nomeUsuario, anoTemporada);
            foreach (var id in idEquipe)
            {
                var equipeRemover = await _context.EquipeTemporadas.Where(x => x.EquipeId == Convert.ToInt32(id) && x.TemporadaId == temporadaId).FirstOrDefaultAsync();
                if (equipeRemover != null)
                {
                    _context.Remove(equipeRemover);
                    await _context.SaveChangesAsync();
                }
            }
            return new JsonResult(Ok());
        }


        public async Task<IActionResult> AdicionarPilotosEquipe(int anoTemporada)
        {

            TempData["anoTemporada"] = anoTemporada;
            var temporada = await _context.Temporadas.Where(x => x.CampeonatoId == _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(nomeUsuario) && x.AnoTemporada == anoTemporada).FirstOrDefaultAsync();
            ViewBag.pilotosTemporadaSemEquipe = _pilotoRepository.PreencherListaDePilotosTemporadaSemEquipe(temporada.TemporadaId);
            ViewBag.equipesTemporada = _equipeRepository.PreencherListaEquipesAdicionadas(temporada.TemporadaId);
            ViewBag.pilotoEquipe = _equipeRepository.PreencherListaPilotosEquipe(temporada.TemporadaId);
            //var equipesNaoAdicionadas = PreencherListaEquipesNaoAdicionadas(equipesAdicionadas, equipesCadastradas);

            //ViewBag.equipesCadastradas = equipesCadastradas;
            //ViewBag.equipesNaoAdicionadas = equipesNaoAdicionadas;
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> PostAddPilotosEquipe(List<string> idPiloto, int equipeId, int anoTemporada, [Bind("Id,PilotoId,EquipeId")] PilotoEquipe pilotoEquipe)
        {
            //var temporadaId = _temporadaRepository.BuscarIdTemporadaPorNomeUsuario(nomeUsuario, anoTemporada);

            if (ModelState.IsValid)
            {
                foreach (var id in idPiloto)
                {
                    pilotoEquipe.Id = 0;
                    pilotoEquipe.EquipeId = equipeId;
                    pilotoEquipe.PilotoId = Convert.ToInt32(id);
                    _context.Add(pilotoEquipe);
                    await _context.SaveChangesAsync();
                }
            }

            return new JsonResult(Ok());
        }

        [HttpPost]
        public async Task<JsonResult> PostRemoverPilotosEquipe(List<string> idPiloto, List<string> idEquipe)
        {
            //var temporadaId = _temporadaRepository.BuscarIdTemporadaPorNomeUsuario(nomeUsuario, anoTemporada);

            if (ModelState.IsValid)
            {
                for (int i = 0; i < idPiloto.Count; i++)
                {
                    var piloto = await _context.PilotosEquipes
                        .Where(x => x.PilotoId == Convert.ToInt32(idPiloto[i]) && x.EquipeId == Convert.ToInt32(idEquipe[i]))
                        .FirstOrDefaultAsync();
                    
                    if (piloto != null)
                    {
                        _context.Remove(piloto);
                        await _context.SaveChangesAsync();
                    }
                }
                //foreach (var id in idPiloto)
                //{
                //    var piloto = await _context.PilotosEquipes.Where(x => x.PilotoId == Convert.ToInt32(id) && x.EquipeId == idEquipe).FirstOrDefaultAsync();
                //    if (piloto != null)
                //    {
                //        _context.Remove(piloto);
                //        await _context.SaveChangesAsync();
                //    }
                //}
            }

            return new JsonResult(Ok());
        }

        private List<Equipe> PreencherListaEquipesNaoAdicionadas(List<Equipe> equipesAdicionadas, List<Equipe> equipesCadastradas)
        {
            List<Equipe> equipeNaoAdicionada = new List<Equipe>();
            foreach (var equipe in equipesCadastradas)
            {
                if (!equipesAdicionadas.Contains(equipe))
                {
                    equipeNaoAdicionada.Add(equipe);
                }
            }
            return equipeNaoAdicionada;
        }
        private List<Equipe> PreencherListaEquipesAdicionadas(Temporada temporada)
        {
            List<Equipe> equipesAdicionadas = new List<Equipe>();
            var IdsEquipesAdicionadas = _context.Equipes.Join(_context.EquipeTemporadas,
                e => e.EquipeId,
                et => et.EquipeId, (e, et) => new { e, et })
                .Join(_context.Temporadas,
                x => x.et.TemporadaId,
                t => t.TemporadaId, (e1, t) => new { e1, t })
                .Where(y => y.t.TemporadaId == temporada.TemporadaId).Select(x => x.e1.e.EquipeId)
                .ToList();
            foreach (var item in IdsEquipesAdicionadas)
            {
                equipesAdicionadas.Add(_context.Equipes.Where(x => x.EquipeId == item).FirstOrDefault());
            }
            return equipesAdicionadas;
        }
    }
}
