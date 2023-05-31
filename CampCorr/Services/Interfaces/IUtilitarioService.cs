namespace CampCorr.Services.Interfaces
{
    public interface IUtilitarioService
    {
        string MontaImagem(byte[] imagemBanco);
        byte[] PreparaImagem(IFormFile imagem);
    }
}
