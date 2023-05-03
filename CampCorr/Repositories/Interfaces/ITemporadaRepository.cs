namespace CampCorr.Repositories.Interfaces
{
    public interface ITemporadaRepository
    {
        int BuscarIdTemporadaPorNomeUsuario(string nomeUsuario, int anoTemporada);
    }
}
