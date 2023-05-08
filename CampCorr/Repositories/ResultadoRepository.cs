using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.ViewModels;

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
    }
}
