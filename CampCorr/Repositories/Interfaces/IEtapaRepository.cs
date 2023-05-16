using CampCorr.Models;

namespace CampCorr.Repositories.Interfaces
{
    public interface IEtapaRepository
    {
        int BuscarIdEtapaPorNomeUsuario(string nomeUsuario, string numeroEtapa, int anoTemporada);
        List<Etapa> BuscarListaEtapasCampeonato(int campeonatoId);
        Circuito BuscarKartodromo(int kartodromoId);
        bool ValidarEtapa(int etapaId, DateTime data);
        Etapa BuscarEtapaPorId(int etapaId);
        void ConcluirEtapa(int etapaId);

    }
}
