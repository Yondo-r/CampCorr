using CampCorr.Models;
using CampCorr.ViewModels;

namespace CampCorr.Repositories.Interfaces
{
    public interface ICampeonatoRepository
    {
        IEnumerable<Campeonato> Campeonatos { get; }
        void Salvar(Campeonato campeonato);
        void Atualizar(Campeonato campeonato);
        List<Campeonato> ListarCampeonatos();
        Task<int> BuscarIdCampeonatoPorNomeUsuarioAsync(string nomeUsuario);
        int BuscarIdCampeonatoPorNomeUsuario(string nomeUsuario);
        Task<Campeonato> BuscarCampeonatoPorId(int campeonatoId);

    }
}
