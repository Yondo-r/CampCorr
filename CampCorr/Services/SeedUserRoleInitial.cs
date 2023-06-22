using CampCorr.Models;
using CampCorr.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampCorr.Services
{
    public class SeedUserRoleInitial : ISeedUserRoleInitial
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICircuitoService _circuitoService;

        public SeedUserRoleInitial(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ICircuitoService circuitoService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _circuitoService = circuitoService;
        }

        public void SeedRoles()
        {
            if (!_roleManager.RoleExistsAsync("Piloto").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Piloto";
                role.NormalizedName = "PILOTO";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
            if (!_roleManager.RoleExistsAsync("Adm").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Adm";
                role.NormalizedName = "ADM";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
            if (!_roleManager.RoleExistsAsync("Master").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Master";
                role.NormalizedName = "MASTER";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
        }

        public void SeedCircuits()
        {
            List<Circuito> circuitosIniciais = PopulaCircuitosIniciais();
            var circuitosExistentes = _circuitoService.ListarCircuitos();
            foreach (var circuito in circuitosIniciais)
            {
                if (!circuitosExistentes.Select(x => x.Nome).Contains(circuito.Nome))
                {
                    _circuitoService.Add(circuito);
                }
            }
        }

        private List<Circuito> PopulaCircuitosIniciais()
        {
            List<Circuito> circuitos = new List<Circuito>();

            Circuito granjaViana = new Circuito();
            granjaViana.Nome = "KGV - Granja Viana";
            granjaViana.Endereço = "Av. José Giorgi, 440 - Granja Viana, Cotia - SP, 06707-100";
            circuitos.Add(granjaViana);

            Circuito interlagos = new Circuito();
            interlagos.Nome = "Interlagos";
            interlagos.Endereço = "Av. Sen. Teotônio Vilela, 261 - Interlagos, São Paulo - SP, 04801-010";
            circuitos.Add(interlagos);

            Circuito aldeiaDaSerra = new Circuito();
            aldeiaDaSerra.Nome = "Aldeia da Serra";
            aldeiaDaSerra.Endereço = "Rua B, 100 - Aldeia da Serra, Santana de Parnaíba - SP, 06539-185";
            circuitos.Add(aldeiaDaSerra);

            Circuito arenaItu = new Circuito();
            arenaItu.Nome = "Arena de Itu";
            arenaItu.Endereço = "Estrada Municipal, 1765 - Vila Vivenda, Itu - SP, 13308-330";
            circuitos.Add(arenaItu);

            Circuito novaOdessa = new Circuito();
            novaOdessa.Nome = "Nova Odessa";
            novaOdessa.Endereço = "Av. Carlos Botelho, 555 - Jardim São Manoel, Nova Odessa - SP, 13460-000";
            circuitos.Add(novaOdessa);

            Circuito sanMarino = new Circuito();
            sanMarino.Nome = "San Marino Kart";
            sanMarino.Endereço = "Av. Dom Pedro II, 499 - Jardim Guanciale, Paulínia - SP, 13140-000";
            circuitos.Add(sanMarino);

            Circuito italiaKart = new Circuito();
            italiaKart.Nome = "Itália Kart";
            italiaKart.Endereço = "Av. Pico da Bandeira, 154 - Vila Independência, São José dos Campos - SP, 12231-380";
            circuitos.Add(italiaKart);

            Circuito atibaia = new Circuito();
            atibaia.Nome = "Atibaia";
            atibaia.Endereço = "Rodovia Dom Pedro I, km 69 - Atibaia - SP, 12952-222";
            circuitos.Add(atibaia);

            Circuito speedLand = new Circuito();
            speedLand.Nome = "Speedland";
            speedLand.Endereço = "Rodovia Raposo Tavares, km 23,5 - Granja Viana, Cotia - SP, 06707-000";
            circuitos.Add(speedLand);

            return circuitos;
        }

        //public void SeedUsers()
        //{
        //    if (_userManager.FindByEmailAsync("usuario@localhost").Result == null)
        //    {
        //        IdentityUser user = new IdentityUser();
        //        user.UserName = "usuario@localhost";
        //        user.Email = "usuario@localhost";
        //        user.NormalizedUserName = "USUARIO@LOCALHOST";
        //        user.NormalizedEmail = "USUARIO@LOCALHOST";
        //        user.EmailConfirmed = true;
        //        user.LockoutEnabled = true;
        //        user.SecurityStamp = Guid.NewGuid().ToString();

        //        IdentityResult result = _userManager.CreateAsync(user, "Numsey#2022").Result;

        //        if (result.Succeeded)
        //        {
        //            _userManager.AddToRoleAsync(user, "Member").Wait();
        //        }
        //    }

        //    if (_userManager.FindByEmailAsync("admin@localhost").Result == null)
        //    {
        //        IdentityUser user = new IdentityUser();
        //        user.UserName = "admin@localhost";
        //        user.Email = "admin@localhost";
        //        user.NormalizedUserName = "ADMIN@LOCALHOST";
        //        user.NormalizedEmail = "ADMIN@LOCALHOST";
        //        user.EmailConfirmed = true;
        //        user.LockoutEnabled = true;
        //        user.SecurityStamp = Guid.NewGuid().ToString();

        //        IdentityResult result = _userManager.CreateAsync(user, "Numsey#2022").Result;

        //        if (result.Succeeded)
        //        {
        //            _userManager.AddToRoleAsync(user, "Admin").Wait();
        //        }
        //    }
        //}
    }
}
