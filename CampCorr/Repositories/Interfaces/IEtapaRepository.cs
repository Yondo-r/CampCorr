using CampCorr.Models;

namespace CampCorr.Repositories.Interfaces
{
    public interface IEtapaRepository
    {
        void Salvar(Etapa etapa);
        void Atualizar(Etapa etapa);
        void Remover(Etapa etapa);
        Task<Etapa> BuscarEtapaAsync(string nomeUsuario, string numeroEtapa, int anoTemporada);
        Task<Etapa> BuscarEtapaAsync(int etapaId);
        List<Etapa> BuscarListaEtapasCampeonato(int campeonatoId);
        List<Etapa> ListarEtapasTemporada(int temporadaId);
        Circuito BuscarCircuito(int CircuitoId);
        //bool ValidarEtapa(int etapaId, DateTime data);
        void ConcluirEtapa(int etapaId);
        int BuscarNumeroEtapaAtual(int temporadaId);
        int QuantidadeEtapas(int temporadaId);

    }
}
