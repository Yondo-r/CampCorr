using CampCorr.Models;

namespace CampCorr.Services.Interfaces
{
    public interface ITemporadaService
    {
        void Salvar(Temporada temporada);
        void Atualizar(Temporada temporada);
        List<Temporada> ListarTemporadasDoCampeonato(int campeonatoId);
        Task<Temporada> BuscarTemporadaAsync(int campeonatoId, int anoTemporada);
        Task<Temporada> BuscarTemporadaAsync(int temporadaId);
        Task<int> BuscarIdTemporadaAsync(string nomeUsuario, int anoTemporada);
        bool TemporadaExists(int id);
    }
}
