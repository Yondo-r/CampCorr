using CampCorr.Context;
using CampCorr.Middleware;
using CampCorr.Repositories;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services;
using CampCorr.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Adicione serviços ao container.

var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("DefaultConnection");
}
else
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Configurações padrão de senha
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;
});

// Repositórios
builder.Services.AddTransient<ICampeonatoRepository, CampeonatoRepository>();
builder.Services.AddTransient<ICircuitoRepository, CircuitoRepository>();
builder.Services.AddTransient<IEquipeRepository, EquipeRepository>();
builder.Services.AddTransient<IEtapaRepository, EtapaRepository>();
builder.Services.AddTransient<IPilotoRepository, PilotoRepository>();
builder.Services.AddTransient<IRegulamentoRepository, RegulamentoRepository>();
builder.Services.AddTransient<IResultadoRepository, ResultadoRepository>();
builder.Services.AddTransient<ITemporadaRepository, TemporadaRepository>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<ILogsRepository, LogsRepository>();

// Serviços
builder.Services.AddScoped<ICalculoService, CalculoService>();
builder.Services.AddScoped<ICampeonatoService, CampeonatoService>();
builder.Services.AddScoped<ICircuitoService, CircuitoService>();
builder.Services.AddScoped<IEquipeService, EquipeService>();
builder.Services.AddScoped<IEtapaService, EtapaService>();
builder.Services.AddScoped<IPilotoService, PilotoService>();
builder.Services.AddScoped<IRegulamentoService, RegulamentoService>();
builder.Services.AddScoped<IResultadoService, ResultadoService>();
builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
builder.Services.AddScoped<ITemporadaService, TemporadaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUtilitarioService, UtilitarioService>();
builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddControllersWithViews();

builder.Services.AddMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure o pipeline de solicitação HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // O valor padrão do HSTS é 30 dias. Você pode alterar isso para cenários de produção, consulte https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<LoggingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

void CriarPerfisUsuarios(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<ISeedUserRoleInitial>();
        service.SeedRoles();
        service.SeedCircuits();
        //service.SeedUsers();
    }
}

CriarPerfisUsuarios(app);

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Campeonatos}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "admin/{action=Index}/{id?}",
        defaults: new { controller = "Campeonatos" });

    endpoints.MapControllerRoute(
        name: "piloto",
        pattern: "piloto/{action=Index}/{id?}",
        defaults: new { controller = "Pilotos" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
