using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CampCorr.Models;

namespace CampCorr.Controllers
{
    public class CircuitoController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
