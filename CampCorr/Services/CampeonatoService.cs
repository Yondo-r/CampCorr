using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;

namespace CampCorr.Services
{
    public class CampeonatoService : ICampeonatoService
    {
        private readonly ICampeonatoRepository _campeonatoRepository;

        public CampeonatoService(ICampeonatoRepository campeonatoRepository)
        {
            _campeonatoRepository = campeonatoRepository;
        }
        public void CriarCampeonato(Campeonato campeonato)
        {
            _campeonatoRepository.Salvar(campeonato);
        }
        public List<Campeonato> ListarCampeonatos()
        {
            return _campeonatoRepository.ListarCampeonatos();
        }

        public async Task<int> BuscarIdCampeonatoAsync(string nomeUsuario)
        {
            return await _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuarioAsync(nomeUsuario);
        }
        public int BuscarIdCampeonato(string nomeUsuario)
        {
            return _campeonatoRepository.BuscarIdCampeonatoPorNomeUsuario(nomeUsuario);
        }
        public async Task<Campeonato> BuscarCampeonato(int campeonatoId)
        {
            return await _campeonatoRepository.BuscarCampeonatoPorId(campeonatoId);
        }
        public void Atualizar(Campeonato campeonato)
        {
            _campeonatoRepository.Atualizar(campeonato);
        }
    }
}
