using CampCorr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampCorr.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }

        public DbSet<Campeonato> Campeonatos { get; set; }
        public DbSet<Temporada> Temporadas { get; set; }
        public DbSet<Etapa> Etapas { get; set; }
        public DbSet<Circuito> Circuitos { get; set; }
        public DbSet<Piloto> Pilotos { get; set; }
        public DbSet<Regulamento> Regulamentos { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<EquipeTemporada> EquipeTemporadas { get; set; }
        public DbSet<PilotoTemporada> PilotosTemporadas { get; set; }
        public DbSet<PilotoCampeonato> PilotosCampeonatos { get; set; }
        public DbSet<PilotoEquipe>  PilotosEquipes { get; set; }
        public DbSet<ResultadoCorrida> ResultadosCorrida { get; set; }

    }
}
