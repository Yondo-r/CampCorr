using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;

namespace CampCorr.Services
{
    public class EtapaService : IEtapaService
    {
        private readonly IEtapaRepository _etapaRepository;

        public EtapaService(IEtapaRepository etapaRepository)
        {
            _etapaRepository = etapaRepository;
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
    }
}
