using CampCorr.Models;

namespace CampCorr.Repositories.Interfaces
{
    public interface ITemporadaRepository
    {
        void Salvar(Temporada temporada);
        void Atualizar(Temporada temporada);
        Task<int> BuscarIdTemporadaPorNomeUsuarioAsync(string nomeUsuario, int anoTemporada);
        List<Temporada> ListarTemporadasDoCampeonato(int campeonatoId);
        Task<Temporada> BuscarTemporadaAsync(int campeonatoId, int anoTemporada);
        Task<Temporada> BuscarTemporadaAsync(int tempodaraId);
        Temporada BuscarTemporada(int temporadaId);
        bool TemporadaExists(int id);
    }
}
