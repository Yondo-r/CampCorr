using CampCorr.Models;
using CampCorr.ViewModels;

namespace CampCorr.Repositories.Interfaces
{
    public interface IEquipeRepository
    {
        public List<Equipe> PreencherListaEquipesAdicionadas(int temporadaId);
        public List<ResultadoCorridaViewModel> PreencherListaPilotosEquipe(int temporadaId);
        public Equipe BuscarEquipe(int idEtapa, int idPiloto);
    }
}
