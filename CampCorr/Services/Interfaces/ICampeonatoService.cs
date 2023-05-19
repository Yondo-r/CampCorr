using CampCorr.Models;

namespace CampCorr.Services.Interfaces
{
    public interface ICampeonatoService
    {
        void CriarCampeonato(Campeonato campeonato);
        void Atualizar(Campeonato campeonato);
        List<Campeonato> ListarCampeonatos();
        Task<int> BuscarIdCampeonatoAsync(string nomeUsuario);
        int BuscarIdCampeonato(string nomeUsuario);
        Task<Campeonato> BuscarCampeonato(int campeonatoId);
    }
}
