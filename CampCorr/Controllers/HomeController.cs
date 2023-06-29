using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using CampCorr.Models;
using Microsoft.AspNetCore.Authorization;
using ReflectionIT.Mvc.Paging;
using System.Diagnostics;
using CampCorr.ViewModels;
using MockQueryable.Moq;
using CampCorr.Services.Interfaces;

namespace CampCorr.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICampeonatoService _campeonatoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IUtilitarioService _utilitarioService;
        private readonly IEtapaService _etapaService;

        public HomeController(ICampeonatoService campeonatoService, IUsuarioService usuarioService, IUtilitarioService utilitarioService, IEtapaService etapaService)
        {
            _campeonatoService = campeonatoService;
            _usuarioService = usuarioService;
            _utilitarioService = utilitarioService;
            _etapaService = etapaService;
        }

        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "NomeCampeonato")
        {
            var usuario = _usuarioService.ListarUsuarios();
            var listaCampeonato = _campeonatoService.ListarCampeonatos();
            var resultadoVm = new List<CampeonatoViewModel>();

            
            foreach (var item in usuario)
            {
                var campeonato = listaCampeonato.Where(x => x.UserId == item.Id).FirstOrDefault();
                if (campeonato != null)
                {
                    resultadoVm.AddRange(new[]
                    {
                        new CampeonatoViewModel(campeonato.CampeonatoId, item.Id, item.UserName, _utilitarioService.MontaImagem(campeonato.Logo))
                    });
                }
            }

            var resultado = resultadoVm.AsQueryable();


            if (!string.IsNullOrWhiteSpace(filter))
            {
                resultado = resultado.Where(c => c.NomeCampeonato.Contains(filter));

            }
            resultado = resultado.BuildMockDbSet().Object;



            var model = await PagingList.CreateAsync(resultado, 5, pageindex, sort, "NomeCampeonato");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Details(string nomeCampeonato)
        {
            var campeonato = await _campeonatoService.BuscarCampeonato(await _campeonatoService.BuscarIdCampeonatoAsync(nomeCampeonato));
            CampeonatoViewModel campeonatoVM = new CampeonatoViewModel(campeonato.CampeonatoId, campeonato.UserId, nomeCampeonato, _utilitarioService.MontaImagem(campeonato.Logo));
            campeonatoVM.Etapas = _etapaService.ListarEtapasCampeonato(campeonato.CampeonatoId);

            return View(campeonatoVM);
        }
    }
}