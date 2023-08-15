using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;

namespace CampCorr.Services
{
    public class RegulamentoService : IRegulamentoService
    {
        private readonly IRegulamentoRepository _regulamentoRepository;

        public RegulamentoService(IRegulamentoRepository regulamentoRepository)
        {
            _regulamentoRepository = regulamentoRepository;
        }

        public Regulamento BuscarRegulamento(int regulamentoId)
        {
            return _regulamentoRepository.BuscarRegulamento(regulamentoId);
        }
        public Regulamento BuscarRegulamentoPorEtapa(int etapaId)
        {
            return _regulamentoRepository.BuscarRegulamentoPorEtapaId(etapaId);
        }
        public List<Regulamento> ListarRegulamentos()
        {
            return  _regulamentoRepository.ListarRegulamentos();
        }
    }
}
