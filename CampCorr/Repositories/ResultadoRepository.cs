using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CampCorr.Repositories
{
    public class ResultadoRepository : IResultadoRepository
    {
        private readonly AppDbContext _context;
        private readonly IEquipeRepository _equipeRepository;

        public ResultadoRepository(AppDbContext context, IEquipeRepository equipeRepository)
        {
            _context = context;
            _equipeRepository = equipeRepository;
        }

        public ResultadoCorridaViewModel BuscarPilotoResultadoEtapa(int etapaId, int pilotoId)
        {
            var resultadoCorrida = _context.ResultadosCorrida.Where(x => x.EtapaId == etapaId && x.PilotoId == pilotoId).FirstOrDefault();
            var piloto = _context.Pilotos.Find(pilotoId);
            var etapa = _context.Etapas.Find(etapaId);
            var equipe = _equipeRepository.BuscarEquipe(etapaId, pilotoId);
            ResultadoCorridaViewModel resultadoVm = new ResultadoCorridaViewModel()
            {
                NomePiloto = piloto.Nome,
                PilotoId = pilotoId,
                EtapaId = etapaId,
                NomeEquipe = equipe.Nome,
                EquipeId = equipe.EquipeId,
                Resultado = resultadoCorrida
            };
            if (resultadoCorrida != null)
            {
                var nomeEquipe = _context.Equipes.Where(x => x.EquipeId == resultadoCorrida.EquipeId).Select(x => x.Nome).FirstOrDefault();
                resultadoVm.ResultadoId = resultadoCorrida.ResultadoId;
                resultadoVm.NomeEquipe = nomeEquipe;
                resultadoVm.Posicao = resultadoCorrida.Posicao;
                resultadoVm.PosicaoLargada = resultadoCorrida.PosicaoLargada;
                resultadoVm.Pontos = resultadoCorrida.Pontos;
                resultadoVm.PontosPenalidade = resultadoCorrida.PontosPenalidade;
                resultadoVm.DescricaoPenalidade = resultadoCorrida.DescricaoPenalidade;
                resultadoVm.MelhorVolta = resultadoCorrida.MelhorVolta;
                resultadoVm.TempoMelhorVolta = resultadoCorrida.TempoMelhorVolta;
                resultadoVm.TempoTotal = resultadoCorrida.TempoTotal;
                resultadoVm.TotalVoltas = resultadoCorrida.TotalVoltas;
            };

            return resultadoVm;
        }
        public async Task<int> BuscarResultadoIdAsync(int etapaId, int pilotoId)
        {
            return await _context.ResultadosCorrida
                .Where(x => x.EtapaId == etapaId && x.PilotoId == pilotoId)
                .Select(x => x.ResultadoId)
                .FirstOrDefaultAsync();
        }

        public void SalvarResultadoCorrida(ResultadoCorrida resultado)
        {
            _context.Update(resultado);
            _context.SaveChanges();
        }
        public async Task<List<ResultadoCorrida>> ListarResultadoEtapa(int etapaId)
        {
            return await _context.ResultadosCorrida.Where(x => x.EtapaId == etapaId).ToListAsync();
        }
        public List<ResultadoCorrida> ListarResultadoEtapa(int temporadaId, int pilotoId)
        {
            List<ResultadoCorrida> resultadoList = new List<ResultadoCorrida>();
            var etapasId = _context.Etapas.Where(x => x.TemporadaId == temporadaId).Select(x => x.EtapaId).ToList();
            foreach (var id in etapasId)
            {
                var resultadoDaEtapa = _context.ResultadosCorrida.Where(x => x.EtapaId == id && x.PilotoId == pilotoId).FirstOrDefault();
                resultadoList.Add(resultadoDaEtapa);
            }
            return resultadoList;
        }
        public List<ResultadoCorrida> MontaListaResultadoTemporada(int temporadaId)
        {
            List<ResultadoCorrida> listaResultadoTemporada = new List<ResultadoCorrida>();
            var resultadoId = _context.ResultadosCorrida.Join(_context.Etapas,
                rc => rc.EtapaId,
                et => et.EtapaId, (rc, et) => new { rc, et })
                .Join(_context.Temporadas,
                Tp => Tp.et.TemporadaId,
                T => T.TemporadaId, (Tp, T) => new { Tp, T })
                .Where(x => x.T.TemporadaId == temporadaId).Select(x => x.Tp.rc.ResultadoId).ToList();
            foreach (int id in resultadoId)
            {
                listaResultadoTemporada.Add(_context.ResultadosCorrida.Where(x => x.ResultadoId == id).FirstOrDefault());
            }
            return listaResultadoTemporada;
        }
        public List<ResultadoTemporada> MontaListaResultadoFinalTemporada(int temporadaId)
        {
            return _context.ResultadosTemporada.Where(x => x.TemporadaId == temporadaId).ToList();
        }
        public void SalvarResultadosTemporada(List<ResultadoTemporada> resultadosTemporada)
        {
            foreach (var resultado in resultadosTemporada)
            {
                _context.Add(resultado);
                _context.SaveChanges();
            }
        }
    }
}
