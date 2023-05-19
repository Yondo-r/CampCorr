using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;
using CampCorr.ViewModels;

namespace CampCorr.Services
{
    public class EquipeService : IEquipeService
    {
        private readonly IEquipeRepository _equipeRepository;

        public EquipeService(IEquipeRepository equipeRepository)
        {
            _equipeRepository = equipeRepository;
        }
        public void SalvarEquipe(Equipe equipe)
        {
            _equipeRepository.SalvarEquipe(equipe);
        }
        public List<Equipe> BuscarEquipesCampeonato(int campeonatoId)
        {
            return _equipeRepository.BuscarEquipesCampeonato(campeonatoId);
        }
        public async Task<EquipeTemporada> BuscarEquipeTemporada(int equipeId, int temporadaId)
        {
            return await _equipeRepository.BuscarEquipeTemporada(equipeId, temporadaId);
        }
        public Equipe BuscarEquipeDoPiloto(int etapaId, int pilotoId)
        {
            return _equipeRepository.BuscarEquipe(etapaId, pilotoId);
        }
        public List<Equipe> ListaEquipesTemporada(int temporadaId)
        {
            return _equipeRepository.PreencherListaEquipesAdicionadas(temporadaId);
        }
        public List<ResultadoCorridaViewModel> ListarPilotosEquipe(int temporadaId)
        {
            return _equipeRepository.PreencherListaPilotosEquipe(temporadaId);
        }
        public void AdicionarEquipeTemporada(EquipeTemporada equipeTemporada)
        {
            _equipeRepository.SalvarEquipeTemporada(equipeTemporada);
        }
        public void RemoverEquipeTemporada(EquipeTemporada equipeTemporada)
        {
            _equipeRepository.RemoverEquipeTemporada(equipeTemporada);
        }
    }
}
