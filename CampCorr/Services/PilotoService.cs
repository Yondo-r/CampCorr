using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;
using CampCorr.ViewModels;

namespace CampCorr.Services
{
    public class PilotoService : IPilotoService
    {
        private readonly IPilotoRepository _pilotoRepository;

        public PilotoService(IPilotoRepository pilotoRepository)
        {
            _pilotoRepository = pilotoRepository;
        }
        public void SalvarPilotoCampeonato(PilotoCampeonato pilotoCampeonato)
        {
            _pilotoRepository.SalvarPilotoCampeonato(pilotoCampeonato);
        }
        public void RemovePilotoCampeonato(int pilotoId, int campeonatoId)
        {
            _pilotoRepository.RemoverPilotoCampeonato(pilotoId, campeonatoId);
        }
        public void SalvarPilotoEquipe(PilotoEquipe pilotoEquipe)
        {
            _pilotoRepository.SalvarPilotoEquipe(pilotoEquipe);
        }
        public void RemoverPilotoEquipe(PilotoEquipe pilotoEquipe)
        {
            _pilotoRepository.RemoverPilotoEquipe(pilotoEquipe);
        }
        public void SalvarPilotoTemporada(PilotoTemporada pilotoTemporada)
        {
            _pilotoRepository.SalvarPilotoTemporada(pilotoTemporada);
        }
        public void RemoverPilotoTemporada(PilotoTemporada pilotoTemporada)
        {
            _pilotoRepository.RemoverPilotoTemporada(pilotoTemporada);
        }
        public async Task<PilotoEquipe> BuscarPilotoEquipeAsync(int pilotoId, int equipeId)
        {
            return await _pilotoRepository.BuscarPilotoEquipeAsync(pilotoId, equipeId);
        }
        public async Task<PilotoTemporada> BuscarPilotoTemporadaAsync(int pilotoId, int temporadaId)
        {
            return await _pilotoRepository.BuscarPilotoTemporadaAsync(pilotoId, temporadaId);
        }
        public List<Piloto> ListarPilotos()
        {
            return _pilotoRepository.ListarPilotos();
        }

        public List<Piloto> ListarPilotosDoCampeonato(int campeonatoId)
        {
            return _pilotoRepository.BuscarPilotosCadastradosCampeonato(campeonatoId);
        }
        public Piloto BuscarPiloto(int pilotoId)
        {
            return _pilotoRepository.BuscarPiloto(pilotoId);
        }
        public List<PilotoCampeonato> ListarPilotosCampeonato()
        {
            return _pilotoRepository.ListarPilotosCampeonato();
        }
        public List<PilotoViewModel> ListarPilotosVmCampeonato(int campeonatoId)
        {
            return _pilotoRepository.ListarPilotoVmCampeonato(campeonatoId);
        }
        public List<PilotoViewModel> ListarPilotosVmTemporada(int temporadaId)
        {
            return _pilotoRepository.PreencherListaDePilotosTemporada(temporadaId);
        }
        public List<PilotoViewModel> ListarPilotosVmSemTemporada(List<PilotoViewModel> listaPilotoCampeonatoVM, List<PilotoViewModel> listaPilotosTemporadaAdicionado)
        {
            return _pilotoRepository.PreencherListaDePilotosNaoAdicionadosTemporada(listaPilotoCampeonatoVM, listaPilotosTemporadaAdicionado);
        }
        public List<Piloto> MontaListaPilotosTemporada(int temporadaId)
        {
            return _pilotoRepository.MontaListaPilotosTemporada(temporadaId).ToList();
        }

        public List<PilotoViewModel> ListarPilotosTemporadaSemEquipe(int temporadaId)
        {
            List<PilotoViewModel> listaPilotosTemporada = _pilotoRepository.PreencherListaDePilotosTemporada(temporadaId);

            var listaPilotoComEquipe = _pilotoRepository.PreencherListaDePilotosTemporadaComEquipe(temporadaId);

            foreach (var piloto in listaPilotoComEquipe)
            {
                if (listaPilotosTemporada.Select(x => x.PilotoId).Contains(piloto.PilotoId))
                {
                    listaPilotosTemporada.Remove(listaPilotosTemporada.Where(x => x.PilotoId == piloto.PilotoId).FirstOrDefault());
                }
            }
            return listaPilotosTemporada;
        }
    }
}
