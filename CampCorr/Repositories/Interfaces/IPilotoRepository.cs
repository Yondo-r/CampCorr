using CampCorr.Models;
using CampCorr.ViewModels;

namespace CampCorr.Repositories.Interfaces
{
    public interface IPilotoRepository
    {
        List<Piloto> BuscarPilotosCadastradosCampeonato(int campeonatoId);
        int BuscarIdPilotoComUsuario(string idUsuario);
        List<PilotoViewModel> PreencherListaDePilotosNaoAdicionadosTemporada(List<PilotoViewModel> listaPilotoCampeonatoVM, List<PilotoViewModel> listaPilotosTemporadaAdicionado);
        List<PilotoViewModel> PreencherListaDePilotosTemporada(int temporadaId);
        public List<ResultadoCorridaViewModel> PreencherListaDePilotosTemporadaComEquipe(int temporadaId);
        public List<PilotoViewModel> PreencherListaDePilotosTemporadaSemEquipe(int temporadaId);
        public Piloto BuscarPilotoPorId(int pilotId);
    }
}
