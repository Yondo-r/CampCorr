using Microsoft.AspNetCore.Mvc;
using CampCorr.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CampCorr.Models;

namespace CampCorr.Controllers
{
    public class KartodromosController : Controller
    {
        private readonly AppDbContext _context;

        public KartodromosController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
