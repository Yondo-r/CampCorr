using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampCorr.Repositories
{
    public class RegulamentoRepository : IRegulamentoRepository
    {
        private readonly AppDbContext _context;
        public RegulamentoRepository(AppDbContext context)
        {
            _context = context;
        }
        public Regulamento BuscarRegulamentoPorEtapaId(int etapaId)
        {
            var etapa = _context.Etapas.Where(x => x.EtapaId == etapaId).FirstOrDefault(); ;
            var temporada = _context.Temporadas.Where(x => x.TemporadaId == etapa.TemporadaId).FirstOrDefault();
            return _context.Regulamentos.Where(x => x.RegulamentoId == temporada.RegulamentoId).FirstOrDefault();
        }
        public Regulamento BuscarRegulamento(int regulamentoId)
        {
            return _context.Regulamentos.Where(x => x.RegulamentoId == regulamentoId).FirstOrDefault();
        }
        public List<Regulamento> ListarRegulamentos()
        {
            return  _context.Regulamentos.ToList();
        }
    }
}
