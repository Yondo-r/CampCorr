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
            granjaViana.Tipo = "Kartodromo";
            circuitos.Add(granjaViana);

            Circuito interlagos = new Circuito();
            interlagos.Nome = "Interlagos";
            interlagos.Endereço = "Av. Sen. Teotônio Vilela, 261 - Interlagos, São Paulo - SP, 04801-010";
            interlagos.Tipo = "Kartodromo";
            circuitos.Add(interlagos);

            Circuito aldeiaDaSerra = new Circuito();
            aldeiaDaSerra.Nome = "Aldeia da Serra";
            aldeiaDaSerra.Endereço = "Rua B, 100 - Aldeia da Serra, Santana de Parnaíba - SP, 06539-185";
            aldeiaDaSerra.Tipo = "Kartodromo";
            circuitos.Add(aldeiaDaSerra);

            Circuito arenaItu = new Circuito();
            arenaItu.Nome = "Arena de Itu";
            arenaItu.Endereço = "Estrada Municipal, 1765 - Vila Vivenda, Itu - SP, 13308-330";
            arenaItu.Tipo = "Kartodromo";
            circuitos.Add(arenaItu);

            Circuito novaOdessa = new Circuito();
            novaOdessa.Nome = "Nova Odessa";
            novaOdessa.Endereço = "Av. Carlos Botelho, 555 - Jardim São Manoel, Nova Odessa - SP, 13460-000";
            novaOdessa.Tipo = "Kartodromo";
            circuitos.Add(novaOdessa);

            Circuito sanMarino = new Circuito();
            sanMarino.Nome = "San Marino Kart";
            sanMarino.Endereço = "Av. Dom Pedro II, 499 - Jardim Guanciale, Paulínia - SP, 13140-000";
            sanMarino.Tipo = "Kartodromo";
            circuitos.Add(sanMarino);

            Circuito italiaKart = new Circuito();
            italiaKart.Nome = "Itália Kart";
            italiaKart.Endereço = "Av. Pico da Bandeira, 154 - Vila Independência, São José dos Campos - SP, 12231-380";
            italiaKart.Tipo = "Kartodromo";
            circuitos.Add(italiaKart);

            Circuito atibaia = new Circuito();
            atibaia.Nome = "Atibaia";
            atibaia.Endereço = "Rodovia Dom Pedro I, km 69 - Atibaia - SP, 12952-222";
            atibaia.Tipo = "Kartodromo";
            circuitos.Add(atibaia);

            Circuito speedLand = new Circuito();
            speedLand.Nome = "Speedland";
            speedLand.Endereço = "Rodovia Raposo Tavares, km 23,5 - Granja Viana, Cotia - SP, 06707-000";
            speedLand.Tipo = "Kartodromo";
            circuitos.Add(speedLand);

            Circuito australia = new Circuito();
            australia.Nome = "Austrália – Melbourne";
            australia.Endereço = "Albert Park Grand Prix Circuit, Melbourne, Victoria, Austrália";
            australia.Tipo = "Autodromo";
            circuitos.Add(australia);

            Circuito bahrein = new Circuito();
            bahrein.Nome = "Bahrein – Sakhir";
            bahrein.Endereço = "Bahrain International Circuit, Sakhir, Bahrein";
            bahrein.Tipo = "Autodromo";
            circuitos.Add(bahrein);

            Circuito china = new Circuito();
            china.Nome = "China – Xangai";
            china.Endereço = "Shanghai International Circuit, Xangai, China";
            china.Tipo = "Autodromo";
            circuitos.Add(china);

            Circuito espanha = new Circuito();
            espanha.Nome = "Espanha – Barcelona";
            espanha.Endereço = "Circuit de Barcelona-Catalunya, Montmeló, Espanha";
            espanha.Tipo = "Autodromo";
            circuitos.Add(espanha);

            Circuito monaco = new Circuito();
            monaco.Nome = "Mônaco – Monte Carlo";
            monaco.Endereço = "Circuit de Monaco, Monte Carlo, Mônaco";
            monaco.Tipo = "Autodromo";
            circuitos.Add(monaco);

            Circuito canada = new Circuito();
            canada.Nome = "Canadá – Montreal";
            canada.Endereço = "Circuit Gilles Villeneuve, Montreal, Canadá";
            canada.Tipo = "Autodromo";
            circuitos.Add(canada);

            Circuito frança = new Circuito();
            frança.Nome = "França – Paul Ricard";
            frança.Endereço = "Circuit Paul Ricard, Le Castellet, França";
            frança.Tipo = "Autodromo";
            circuitos.Add(frança);

            Circuito austria = new Circuito();
            austria.Nome = "Áustria – Spielberg";
            austria.Endereço = "Red Bull Ring, Spielberg, Áustria";
            austria.Tipo = "Autodromo";
            circuitos.Add(austria);

            Circuito inglaterra = new Circuito();
            inglaterra.Nome = "Inglaterra – Silverstone";
            inglaterra.Endereço = "Silverstone Circuit, Silverstone, Inglaterra";
            inglaterra.Tipo = "Autodromo";
            circuitos.Add(inglaterra);

            Circuito hungria = new Circuito();
            hungria.Nome = "Hungria – Hungaroring";
            hungria.Endereço = "Hungaroring, Budapeste, Hungria";
            hungria.Tipo = "Autodromo";
            circuitos.Add(hungria);

            Circuito belgica = new Circuito();
            belgica.Nome = "Bélgica – Spa-Francorchamps";
            belgica.Endereço = "Circuit de Spa-Francorchamps, Stavelot, Bélgica";
            belgica.Tipo = "Autodromo";
            circuitos.Add(belgica);

            Circuito holanda = new Circuito();
            holanda.Nome = "Holanda – Zandvoort";
            holanda.Endereço = "Circuit Zandvoort, Zandvoort, Holanda";
            holanda.Tipo = "Autodromo";
            circuitos.Add(holanda);

            Circuito italia = new Circuito();
            italia.Nome = "Itália – Monza";
            italia.Endereço = "Autodromo Nazionale Monza, Monza, Itália";
            italia.Tipo = "Autodromo";
            circuitos.Add(italia);

            Circuito russia = new Circuito();
            russia.Nome = "Rússia – Sochi";
            russia.Endereço = "Sochi Autodrom, Sochi, Rússia";
            russia.Tipo = "Autodromo";
            circuitos.Add(russia);

            Circuito singapore = new Circuito();
            singapore.Nome = "Singapura – Marina Bay";
            singapore.Endereço = "Marina Bay Street Circuit, Singapura";
            singapore.Tipo = "Autodromo";
            circuitos.Add(singapore);

            Circuito japao = new Circuito();
            japao.Nome = "Japão – Suzuka";
            japao.Endereço = "Suzuka International Racing Course, Suzuka, Japão";
            japao.Tipo = "Autodromo";
            circuitos.Add(japao);

            Circuito estadosUnidos = new Circuito();
            estadosUnidos.Nome = "Estados Unidos – Austin";
            estadosUnidos.Endereço = "Circuit of the Americas, Austin, Texas, EUA";
            estadosUnidos.Tipo = "Autodromo";
            circuitos.Add(estadosUnidos);

            Circuito mexico = new Circuito();
            mexico.Nome = "México – Cidade do México";
            mexico.Endereço = "Autódromo Hermanos Rodríguez, Cidade do México, México";
            mexico.Tipo = "Autodromo";
            circuitos.Add(mexico);

            Circuito brasil = new Circuito();
            brasil.Nome = "Brasil – São Paulo";
            brasil.Endereço = "Autódromo José Carlos Pace, São Paulo, Brasil";
            brasil.Tipo = "Autodromo";
            circuitos.Add(brasil);

            Circuito arabiaSaudita = new Circuito();
            arabiaSaudita.Nome = "Arábia Saudita – Jeddah";
            arabiaSaudita.Endereço = "Jeddah Street Circuit, Jeddah, Arábia Saudita";
            arabiaSaudita.Tipo = "Autodromo";
            circuitos.Add(arabiaSaudita);

            Circuito abuDhabi = new Circuito();
            abuDhabi.Nome = "Abu Dhabi – Yas Marina";
            abuDhabi.Endereço = "Yas Marina Circuit, Yas Island, Abu Dhabi, Emirados Árabes Unidos";
            abuDhabi.Tipo = "Autodromo";
            circuitos.Add(abuDhabi);


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
