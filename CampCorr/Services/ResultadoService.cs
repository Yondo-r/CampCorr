using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;
using CampCorr.ViewModels;

namespace CampCorr.Services
{
    public class ResultadoService : IResultadoService
    {
        private readonly IResultadoRepository _resultadoRepository;
        private readonly ITemporadaRepository _temporadaRepository;
        private readonly IPilotoService _pilotoService;
        private readonly IEquipeService _equipeService;

        public ResultadoService(IResultadoRepository resultadoRepository, ITemporadaRepository temporadaRepository, IPilotoService pilotoService, IEquipeService equipeService)
        {
            _resultadoRepository = resultadoRepository;
            _temporadaRepository = temporadaRepository;
            _pilotoService = pilotoService;
            _equipeService = equipeService;
        }
        public void Salvar(ResultadoCorrida resultadoCorrida)
        {
            _resultadoRepository.SalvarResultadoCorrida(resultadoCorrida);
        }
        public async Task<int> BuscarResultadoIdAsync(int etapaId, int pilotoId)
        {
            return await _resultadoRepository.BuscarResultadoIdAsync(etapaId, pilotoId);
        }
        public ResultadoCorridaViewModel BuscarPilotoResultadoEtapa(int etapaId, int pilotoId)
        {
            return _resultadoRepository.BuscarPilotoResultadoEtapa(etapaId, pilotoId);
        }
        public async Task<List<ResultadoCorrida>> ListarResultadoEtapa(int etapaId)
        {
            return await _resultadoRepository.ListarResultadoEtapa(etapaId);
        }
        public List<ResultadoCorrida> MontaListaResultadoTemporada(int temporadaId)
        {
            return _resultadoRepository.MontaListaResultadoTemporada(temporadaId);
        }
        public List<ResultadoTemporada> MontaListaResultadoFinalTemporada(int temporadaId)
        {
            return _resultadoRepository.MontaListaResultadoFinalTemporada(temporadaId);
        }
        public void ConcluirTemporada(List<ResultadoTemporada> resultadosTemporada, Temporada temporada)
        {
            _resultadoRepository.SalvarResultadosTemporada(resultadosTemporada);
            temporada.Concluida = true;
            _temporadaRepository.Atualizar(temporada);
        }

        public List<ResultadoCorridaViewModel> MontaResultadoTemporadaParcial(int temporadaId)
        {
            List<ResultadoCorridaViewModel> tabelaResultado = new List<ResultadoCorridaViewModel>();
            List<ResultadoCorrida> resultadoTemporadaParcial = MontaListaResultadoTemporada(temporadaId);
            List<CampCorr.Models.Piloto> listaPilotos = _pilotoService.MontaListaPilotosTemporada(temporadaId);

            foreach (var piloto in listaPilotos)
            {
                int pontos = 0;
                int vitorias = 0;
                var listaResultadosDoPiloto = resultadoTemporadaParcial.Where(x => x.PilotoId == piloto.PilotoId).ToList();

                //Soma os pontos do piloto
                foreach (var resultadoDoPiloto in listaResultadosDoPiloto)
                {
                    pontos += (int)resultadoDoPiloto.Pontos;

                    //Verifica quantas vitórias o piloto possui. 
                    if (resultadoDoPiloto.Posicao == 1)
                    {
                        vitorias++;
                    }
                }
                ResultadoCorridaViewModel resultadoPiloto = new ResultadoCorridaViewModel()
                {
                    NomePiloto = piloto.Nome,
                    NomeEquipe = _equipeService.BuscarEquipeDoPiloto(listaResultadosDoPiloto[0].EtapaId, piloto.PilotoId).Nome,
                    Pontos = pontos,
                    NumeroVitorias = vitorias,
                };
                tabelaResultado.Add(resultadoPiloto);
            }
            //tabelaResultado = (List<ResultadoCorridaViewModel>)tabelaResultado.OrderByDescending(x => x.Pontos);
            return tabelaResultado;
        }

        public List<ResultadoCorridaViewModel> MontaListaResultadoVm(Etapa etapa)
        {
            List<ResultadoCorridaViewModel> listaResultadoVm = new List<ResultadoCorridaViewModel>();
            List<PilotoViewModel> listaPilotos = _pilotoService.ListarPilotosVmTemporada(etapa.TemporadaId);

            foreach (var piloto in listaPilotos)
            {
                var resultado = BuscarPilotoResultadoEtapa(etapa.EtapaId, piloto.PilotoId);
                listaResultadoVm.Add(resultado);
            }

            return listaResultadoVm;
        }
    }
}
