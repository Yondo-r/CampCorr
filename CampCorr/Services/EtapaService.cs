using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;

namespace CampCorr.Services
{
    public class EtapaService : IEtapaService
    {
        private readonly IEtapaRepository _etapaRepository;
        private readonly ITemporadaRepository _temporadaRepository;

        public EtapaService(IEtapaRepository etapaRepository, ITemporadaRepository temporadaRepository)
        {
            _etapaRepository = etapaRepository;
            _temporadaRepository = temporadaRepository;
        }
        public void Salvar(Etapa etapa)
        {
            _etapaRepository.Salvar(etapa);
        }
        public void Atualizar(Etapa etapa)
        {
            _etapaRepository.Atualizar(etapa);
        }
        public void Remover(Etapa etapa)
        {
            _etapaRepository.Remover(etapa);
        }
        public int BuscarNumeroEtapaAtual(int temporadaId)
        {
            return _etapaRepository.BuscarNumeroEtapaAtual(temporadaId);
        }
        public int QuantidadeEtapas(int temporadaId)
        {
            return _etapaRepository.QuantidadeEtapas(temporadaId);
        }
        public async Task<Etapa> BuscarEtapaAsync(string nomeUsuario, string numeroEtapa, int ano)
        {
            return await _etapaRepository.BuscarEtapaAsync(nomeUsuario, numeroEtapa, ano);
        }
        public async Task<Etapa> BuscarEtapaAsync(int etapaId)
        {
            return await _etapaRepository.BuscarEtapaAsync(etapaId);
        }
        public Etapa BuscarEtapa(int etapaId)
        {
            return _etapaRepository.BuscarEtapa(etapaId);
        }
        public List<Etapa> ListarEtapasTemporada(int temporadaId)
        {
            return _etapaRepository.ListarEtapasTemporada(temporadaId);
        }
        public List<Etapa> ListarEtapasCampeonato(int campeonatoId)
        {
            return _etapaRepository.BuscarListaEtapasCampeonato(campeonatoId);
        }
        public void ConcluirEtapa(int etapaId)
        {
            _etapaRepository.ConcluirEtapa(etapaId);
        }

        public string NavegarEtapas(string numeroEtapa, int navegacao)
            //deverá receber um valor no formato (ex: "1 de 4" , -1)
        {
            string proximaEtapa;
            int valorNumeroEtapa = Convert.ToInt32(numeroEtapa.Substring(0, 1));
            int valorProximaEtapa = valorNumeroEtapa + navegacao;
            proximaEtapa = Convert.ToString(valorProximaEtapa) + numeroEtapa.Substring(1);

            return proximaEtapa;
        }

        public bool VerificaSeUltimaEtapa(int etapaId)
        {
            var etapa = _etapaRepository.BuscarEtapa(etapaId);
            var temporada = _temporadaRepository.BuscarTemporada(etapa.TemporadaId);
            var etapaTemporada = Convert.ToInt32(etapa.NumeroEvento.Substring(0, 1));
            if (temporada.QuantidadeEtapas == etapaTemporada)
            {
                return true;
            }
            return false;
        }
    }
}
