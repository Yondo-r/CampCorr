using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;
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
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly string nomeUsuario;
        private readonly int campeonatoId;
        private readonly ICampeonatoService _campeonatoService;
        private readonly IEquipeService _equipeService;
        private readonly ITemporadaService _temporadasService;
        private readonly IPilotoService _pilotoService;

        public EquipesController(SignInManager<IdentityUser> signInManager, ICampeonatoService campeonatoService, IEquipeService equipeService, ITemporadaService temporadasService, IPilotoService pilotoService)
        {
            _signInManager = signInManager;
            _campeonatoService = campeonatoService;
            _equipeService = equipeService;
            _temporadasService = temporadasService;
            _pilotoService = pilotoService;
            nomeUsuario = signInManager.Context.User.Identity.Name;
            campeonatoId = campeonatoService.BuscarIdCampeonato(nomeUsuario);
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
            equipe.CampeonatoId =  await _campeonatoService.BuscarIdCampeonatoAsync(nomeUsuario);
            if (ModelState.IsValid)
            {
                _equipeService.SalvarEquipe(equipe);
                return RedirectToAction("VisualizarEquipes", new { anoTemporada = anoTemporada });
            }
            return View();
        }
        public async Task<IActionResult> VisualizarEquipes(int anoTemporada)
        {
            TempData["anoTemporada"] = anoTemporada;
            var temporada = await _temporadasService.BuscarTemporadaAsync(campeonatoId, anoTemporada);
            var equipesCadastradas = _equipeService.BuscarEquipesCampeonato(campeonatoId);
            var equipesAdicionadas = _equipeService.ListaEquipesTemporada(temporada.TemporadaId);
            var equipesNaoAdicionadas = PreencherListaEquipesNaoAdicionadas(equipesAdicionadas, equipesCadastradas);

            ViewBag.equipesCadastradas = equipesCadastradas;
            ViewBag.equipesAdicionadas = equipesAdicionadas;
            ViewBag.equipesNaoAdicionadas = equipesNaoAdicionadas;
            return View(equipesCadastradas);
        }



        [HttpPost]
        public async Task<JsonResult> PostAddEquipes(List<string> idEquipe, int anoTemporada, [Bind("Id,EquipeId,TemporadaId")] EquipeTemporada equipeTemporada)
        {
            var temporadaId = await _temporadasService.BuscarIdTemporadaAsync(nomeUsuario, anoTemporada);

            if (ModelState.IsValid)
            {
                foreach (var id in idEquipe)
                {
                    equipeTemporada.Id = 0;
                    equipeTemporada.TemporadaId = temporadaId;
                    equipeTemporada.EquipeId = Convert.ToInt32(id);
                    _equipeService.AdicionarEquipeTemporada(equipeTemporada);
                }
            }
            return new JsonResult(Ok());
        }
        [HttpPost]
        public async Task<JsonResult> PostRemoverEquipes(List<string> idEquipe, int anoTemporada)
        {
            var temporadaId = await _temporadasService.BuscarIdTemporadaAsync(nomeUsuario, anoTemporada);
            foreach (var id in idEquipe)
            {
                var equipeRemover = await _equipeService.BuscarEquipeTemporada(Convert.ToInt32(id), temporadaId);
                if (equipeRemover != null)
                {
                    _equipeService.RemoverEquipeTemporada(equipeRemover);
                }
            }
            return new JsonResult(Ok());
        }


        public async Task<IActionResult> AdicionarPilotosEquipe(int anoTemporada)
        {
            TempData["anoTemporada"] = anoTemporada;
            var temporada = await _temporadasService.BuscarTemporadaAsync(campeonatoId, anoTemporada);
            ViewBag.pilotosTemporadaSemEquipe = _pilotoService.ListarPilotosTemporadaSemEquipe(temporada.TemporadaId);
            ViewBag.equipesTemporada = _equipeService.ListaEquipesTemporada(temporada.TemporadaId);
            ViewBag.pilotoEquipe = _equipeService.ListarPilotosEquipe(temporada.TemporadaId);
           
            return View();
        }
        [HttpPost]
        public JsonResult PostAddPilotosEquipe(List<string> idPiloto, int equipeId, [Bind("Id,PilotoId,EquipeId")] PilotoEquipe pilotoEquipe)
        {
            if (ModelState.IsValid)
            {
                foreach (var id in idPiloto)
                {
                    pilotoEquipe.Id = 0;
                    pilotoEquipe.EquipeId = equipeId;
                    pilotoEquipe.PilotoId = Convert.ToInt32(id);
                    _pilotoService.SalvarPilotoEquipe(pilotoEquipe);
                }
            }

            return new JsonResult(Ok());
        }

        [HttpPost]
        public async Task<JsonResult> PostRemoverPilotosEquipe(List<string> idPiloto, List<string> idEquipe)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < idPiloto.Count; i++)
                {
                    var piloto = await _pilotoService.BuscarPilotoEquipeAsync(Convert.ToInt32(idPiloto[i]), Convert.ToInt32(idEquipe[i]));
                    
                    if (piloto != null)
                    {
                        _pilotoService.RemoverPilotoEquipe(piloto);
                    }
                }
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
    }
}
