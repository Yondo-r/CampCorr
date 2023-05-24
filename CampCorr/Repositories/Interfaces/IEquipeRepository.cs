using CampCorr.Models;
using CampCorr.ViewModels;

namespace CampCorr.Repositories.Interfaces
{
    public interface IEquipeRepository
    {
        List<Equipe> PreencherListaEquipesAdicionadas(int temporadaId);
        List<ResultadoCorridaViewModel> PreencherListaPilotosEquipe(int temporadaId);
        Equipe BuscarEquipe(int equipeId);
        Equipe BuscarEquipe(int idEtapa, int idPiloto);
        Task<EquipeTemporada> BuscarEquipeTemporada(int equipeId, int temporadaId);
        void SalvarEquipe(Equipe equipe);
        void SalvarEquipeTemporada(EquipeTemporada equipeTemporada);
        void RemoverEquipeTemporada(EquipeTemporada equipeTemporada);
        List<Equipe> BuscarEquipesCampeonato(int campeonatoId);
    }
}
