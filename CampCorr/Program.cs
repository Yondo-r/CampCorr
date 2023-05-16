using CampCorr.Context;
using CampCorr.Repositories;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services;
using CampCorr.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    //Default Password Settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;
});


builder.Services.AddTransient<ICampeonatoRepository, CampeonatoRepository>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IPilotoRepository, PilotoRepository>();
builder.Services.AddTransient<ITemporadaRepository, TemporadaRepository>();
builder.Services.AddTransient<IEtapaRepository, EtapaRepository>();
builder.Services.AddTransient<IResultadoRepository, ResultadoRepository>();
builder.Services.AddTransient<IEquipeRepository, EquipeRepository>();
builder.Services.AddTransient<IRegulamentoRepository, RegulamentoRepository>();
//builder.Services.AddTransient<ICalculo, Calculo>();
builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Admin", politica =>
//    {
//        politica.RequireRole("Admin");
//    });
//});


builder.Services.AddControllersWithViews();

builder.Services.AddMemoryCache();
builder.Services.AddSession();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
});


app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{action=Index}/{id?}",
    defaults: new { controller = "Campeonatos" });

app.MapControllerRoute(
    name: "piloto",
    pattern: "piloto/{action=Index}/{id?}",
    defaults: new { controller = "pilotos" });


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
