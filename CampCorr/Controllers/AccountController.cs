using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Services.Interfaces;
using CampCorr.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampCorr.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICampeonatoService _campeonatoService;
        private readonly AppDbContext _context;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext context, ICampeonatoService campeonatoService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _campeonatoService = campeonatoService;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        public async Task<IActionResult> Login (LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(loginVM.ReturnUrl);
                }
            }
            ModelState.AddModelError("", "Falha ao realizar o login!!");
            return View(loginVM);
        }

        

        public IActionResult Register(string area)
        {
            ViewData["area"] = area;
            return View();
        }

        public IActionResult RegisterAdm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginViewModel registroVM, Campeonato campeonato, Piloto piloto)
        {
            if (ModelState.IsValid)
            {
                //Verifica se já existe esse usuário cadastrado
                var usuario = _context.Users.Where(x => x.UserName.Contains(registroVM.UserName)).FirstOrDefault();
                if (usuario != null)
                {
                    this.ModelState.AddModelError("Registro", "Usuario já cadastrado");
                }
                else
                {
                    var user = new IdentityUser { UserName = registroVM.UserName };
                    var result = await _userManager.CreateAsync(user, registroVM.Password);

                    if (result.Succeeded)
                    {
                        if (registroVM.Area == "Adm")
                        {
                            //await _signInManager.SignInAsync(user, isPersistent: false);
                            await _userManager.AddToRoleAsync(user, "Adm");
                            campeonato.UserId = user.Id;
                            _campeonatoService.CriarCampeonato(campeonato);
                            //_context.Add(campeonato);
                            //await _context.SaveChangesAsync();
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, "Piloto");
                            piloto.UsuarioId = user.Id;
                            _context.Add(piloto);
                            await _context.SaveChangesAsync();
                            await _signInManager.PasswordSignInAsync(user, registroVM.Password, false, false);
                            return RedirectToAction("Cadastro", "Pilotos", new { area = "Piloto" } );
                        }
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        this.ModelState.AddModelError("Registro", "Falha ao registrar um usuário");
                    }
                }
            }
            return View(registroVM);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.User = null;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EsqueceuSenha()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EsqueceuSenha(string UserName, string senhaAntiga, string senhaNova, string repeteSenhaNova)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(UserName);

                if (repeteSenhaNova != senhaNova)
                {
                    ModelState.AddModelError("erro", "A senha nova digitada é diferente da digitada na repetição");
                    return View();
                }
                if (user == null || !await _userManager.CheckPasswordAsync(user, senhaAntiga))
                {
                    ModelState.AddModelError("erro", "Usuário ou senha inválidos");
                    return View();
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, senhaNova);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.ToString());
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
