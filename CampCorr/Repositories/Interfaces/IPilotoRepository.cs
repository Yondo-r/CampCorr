using CampCorr.Models;
using CampCorr.ViewModels;

namespace CampCorr.Repositories.Interfaces
{
    public interface IPilotoRepository
    {
        void SalvarPilotoCampeonato(PilotoCampeonato pilotoCampeonato);
        void RemoverPilotoCampeonato(int pilotoId, int campeonatoId);
        void SalvarPilotoEquipe(PilotoEquipe pilotoEquipe);
        void RemoverPilotoEquipe(PilotoEquipe pilotoEquipe);
        void SalvarPilotoTemporada(PilotoTemporada pilotoTemporada);
        void RemoverPilotoTemporada(PilotoTemporada pilotoTemporada);
        List<Piloto> ListarPilotos();
        List<PilotoCampeonato> ListarPilotosCampeonato();
        List<PilotoViewModel> ListarPilotoVmCampeonato(int campeonatoId);
        List<Piloto> BuscarPilotosCadastradosCampeonato(int campeonatoId);
        int BuscarIdPilotoComUsuario(string idUsuario);
        List<PilotoViewModel> PreencherListaDePilotosNaoAdicionadosTemporada(List<PilotoViewModel> listaPilotoCampeonatoVM, List<PilotoViewModel> listaPilotosTemporadaAdicionado);
        List<PilotoViewModel> PreencherListaDePilotosTemporada(int temporadaId);
        List<ResultadoCorridaViewModel> PreencherListaDePilotosTemporadaComEquipe(int temporadaId);
        Piloto BuscarPilotoPorId(int pilotId);
        Task<PilotoEquipe> BuscarPilotoEquipeAsync(int pilotoId, int equipeId);
        Task<PilotoTemporada> BuscarPilotoTemporadaAsync(int pilotoId, int temporadaId);
        Piloto BuscarPiloto(int pilotoId);
        List<Piloto> MontaListaPilotosTemporada(int temporadaId);
    }
}


