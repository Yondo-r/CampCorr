using CampCorr.Models;
using CampCorr.ViewModels;

namespace CampCorr.Services.Interfaces
{
    public interface IEquipeService
    {
        void SalvarEquipe(Equipe equipe);
        Equipe BuscarEquipe(int equipeId);
        List<Equipe> BuscarEquipesCampeonato(int campeonatoId);
        Equipe BuscarEquipeDoPiloto(int etapaId, int pilotoId);
        List<Equipe> ListaEquipesTemporada(int temporadaId);
        List<ResultadoCorridaViewModel> ListarPilotosEquipe(int temporadaId);
        void AdicionarEquipeTemporada(EquipeTemporada equipeTemporada);
        Task<EquipeTemporada> BuscarEquipeTemporada(int equipeId, int temporadaId);
        void RemoverEquipeTemporada(EquipeTemporada equipeTemporada);
    
    }
}
