﻿using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CampCorr.Repositories
{
    public class EquipeRepository : IEquipeRepository
    {
        private readonly AppDbContext _context;

        public EquipeRepository(AppDbContext context)
        {
            _context = context;
        }
        public void SalvarEquipe(Equipe equipe)
        {
            _context.Add(equipe);
            _context.SaveChanges();
        }
        public void SalvarEquipeTemporada(EquipeTemporada equipeTemporada)
        {
            _context.Add(equipeTemporada);
            _context.SaveChanges();
        }
        public void RemoverEquipeTemporada(EquipeTemporada equipeTemporada)
        {
            _context.Remove(equipeTemporada);
            _context.SaveChanges();
        }

        //Esse método lista todas as equipes que estão cadastradas na temporada
        public List<Equipe> PreencherListaEquipesAdicionadas(int temporadaId)
        {
            List<Equipe> equipesAdicionadas = new List<Equipe>();
            var IdsEquipesAdicionadas = _context.Equipes.Join(_context.EquipeTemporadas,
                e => e.EquipeId,
                et => et.EquipeId, (e, et) => new { e, et })
                .Join(_context.Temporadas,
                x => x.et.TemporadaId,
                t => t.TemporadaId, (e1, t) => new { e1, t })
                .Where(y => y.t.TemporadaId == temporadaId).Select(x => x.e1.e.EquipeId)
                .ToList();
            foreach (var item in IdsEquipesAdicionadas)
            {
                equipesAdicionadas.Add(_context.Equipes.Where(x => x.EquipeId == item).FirstOrDefault());
            }
            return equipesAdicionadas;
        }

        public List<ResultadoCorridaViewModel> PreencherListaPilotosEquipe(int temporadaId)
        {
            List<ResultadoCorridaViewModel> pilotosEquipe = new List<ResultadoCorridaViewModel>();
            var objPilotoEquipe = _context.PilotosEquipes.Join(_context.EquipeTemporadas,
                pe => pe.EquipeId,
                et => et.EquipeId, (pe, et) => new { pe, et })
                .Where(x => x.et.TemporadaId == temporadaId).Select(x => x.pe)
                .ToList();

            foreach (var item in objPilotoEquipe)
            {
                ResultadoCorridaViewModel equipePiloto = new ResultadoCorridaViewModel()
                {
                    PilotoId = item.PilotoId,
                    EquipeId = item.EquipeId,
                    NomeEquipe = _context.Equipes.Where(x => x.EquipeId == item.EquipeId).Select(x => x.Nome).FirstOrDefault(),
                    NomePiloto = _context.Pilotos.Where(x => x.PilotoId == item.PilotoId).Select(x => x.Nome).FirstOrDefault()
                };

                pilotosEquipe.Add(equipePiloto);
            }
            return pilotosEquipe;
        }

        public Equipe BuscarEquipe(int equipeId)
        {
            return _context.Equipes.FirstOrDefault(x => x.EquipeId == equipeId);
        }
        public Equipe BuscarEquipe(int idEtapa, int idPiloto)
        {
            var temporadaId = _context.Etapas.Where(x => x.EtapaId == idEtapa).Select(x => x.TemporadaId).FirstOrDefault();
            var query = _context.Equipes.Join(_context.EquipeTemporadas,
                e => e.EquipeId,
                et => et.EquipeId, (e, et) => new { e, et })
                .Join(_context.PilotosEquipes,
                te => te.et.EquipeId,
                pe => pe.EquipeId, (te, pe) => new { te, pe })
                .Where(x => x.pe.PilotoId == idPiloto && x.te.et.TemporadaId == temporadaId)
                .FirstOrDefault();
            var equipe = query.te.e;
            return equipe;
        }

        public List<Equipe> BuscarEquipesCampeonato(int campeonatoId)
        {
            return _context.Equipes.Where(x => x.CampeonatoId == campeonatoId).ToList();
        }

        public async Task<EquipeTemporada> BuscarEquipeTemporada(int equipeId, int temporadaId)
        {
            return await _context.EquipeTemporadas.Where(x => x.EquipeId == equipeId && x.TemporadaId == temporadaId).FirstOrDefaultAsync();
        }
    }
}
