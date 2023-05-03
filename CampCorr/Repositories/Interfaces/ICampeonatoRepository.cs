using CampCorr.Models;
using CampCorr.ViewModels;

namespace CampCorr.Repositories.Interfaces
{
    public interface ICampeonatoRepository
    {
        IEnumerable<Campeonato> Campeonatos { get; }
        void Salvar(Campeonato campeonato);
        void RemovePilotoCampeonato(int idPiloto, int idCampeonato);
        int BuscarIdCampeonatoPorNomeUsuario(string nomeUsuario);


    }
}
