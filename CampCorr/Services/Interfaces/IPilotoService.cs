using CampCorr.Models;
using CampCorr.ViewModels;

namespace CampCorr.Services.Interfaces
{
    public interface IPilotoService
    {
        void SalvarPilotoCampeonato(PilotoCampeonato pilotoCampeonato);
        void RemovePilotoCampeonato(int pilotoId, int campeonatoId);
        void SalvarPilotoEquipe(PilotoEquipe pilotoEquipe);
        void RemoverPilotoEquipe(PilotoEquipe pilotoEquipe);
        void SalvarPilotoTemporada(PilotoTemporada pilotoTemporada);
        void RemoverPilotoTemporada(PilotoTemporada pilotoTemporada);
        Task<PilotoEquipe> BuscarPilotoEquipeAsync(int pilotoId, int equipeId);
        Task<PilotoTemporada> BuscarPilotoTemporadaAsync(int pilotoId, int temporadaId);
        Piloto BuscarPiloto(int pilotoId);
        List<Piloto> ListarPilotos();
        List<Piloto> ListarPilotosDoCampeonato(int campeonatoId);
        List<PilotoCampeonato> ListarPilotosCampeonato();
        List<PilotoViewModel> ListarPilotosVmCampeonato(int campeonatoId);
        List<PilotoViewModel> ListarPilotosVmSemTemporada(List<PilotoViewModel> listaPilotoCampeonatoVM, List<PilotoViewModel> listaPilotosTemporadaAdicionado);
        List<PilotoViewModel> ListarPilotosVmTemporada(int temporadaId);
        List<PilotoViewModel> ListarPilotosTemporadaSemEquipe(int temporadaId);
        List<Piloto> MontaListaPilotosTemporada(int temporadaId);
    }
}
