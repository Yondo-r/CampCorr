using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;

namespace CampCorr.Services
{
    public class TemporadaService : ITemporadaService
    {
        private readonly ITemporadaRepository _temporadaRepository;

        public TemporadaService(ITemporadaRepository temporadaRepository)
        {
            _temporadaRepository = temporadaRepository;
        }
        public void Salvar(Temporada temporada)
        {
            _temporadaRepository.Salvar(temporada);
        }
        public void Atualizar(Temporada temporada)
        {
            _temporadaRepository.Atualizar(temporada);
        }
        public List<Temporada> ListarTemporadasDoCampeonato(int campeonatoId)
        {
            return _temporadaRepository.ListarTemporadasDoCampeonato(campeonatoId);
        }
        public async Task<Temporada> BuscarTemporadaAsync(int campeonatoId, int anoTemporada)
        {
            return await _temporadaRepository.BuscarTemporadaAsync(campeonatoId, anoTemporada);
        }
        public async Task<Temporada> BuscarTemporadaAsync(int temporadaId)
        {
            return await _temporadaRepository.BuscarTemporadaAsync(temporadaId);
        }
        public async Task<int> BuscarIdTemporadaAsync(string nomeUsuario, int anoTemporada)
        {
            return await _temporadaRepository.BuscarIdTemporadaPorNomeUsuarioAsync(nomeUsuario, anoTemporada);
        }
        public bool TemporadaExists(int id)
        {
            return _temporadaRepository.TemporadaExists(id);
        }
    }
}
