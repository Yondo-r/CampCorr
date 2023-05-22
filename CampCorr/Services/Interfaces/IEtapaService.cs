using CampCorr.Models;

namespace CampCorr.Services.Interfaces
{
    public interface IEtapaService
    {
        void Salvar(Etapa etapa);
        void Atualizar(Etapa etapa);
        void Remover(Etapa etapa);
        int BuscarNumeroEtapaAtual(int temporadaId);
        int QuantidadeEtapas(int temporadaId);
        Task<Etapa> BuscarEtapaAsync(string nomeUsuario, string numeroEtapa, int ano);
        Task<Etapa> BuscarEtapaAsync(int etapaId);
        Etapa BuscarEtapa(int etapaId);
        List<Etapa> ListarEtapasTemporada(int temporadaId);
        List<Etapa> ListarEtapasCampeonato(int campeonatoId);
        void ConcluirEtapa(int etapaId);


    }
}
