using CampCorr.Services.Interfaces;

namespace CampCorr.Services
{
    public class UtilitarioService : IUtilitarioService
    {
        //Monta imagem para ser exibida para o usuário
        public string MontaImagem(byte[] imagemBanco)
        {
            if (imagemBanco != null)
            {
                string base64String = Convert.ToBase64String(imagemBanco);
                string imagemDataUrl = $"data:image/jpeg;base64,{base64String}";
                return imagemDataUrl;
            }
            return null;
        }
        //Prepara imagem para ser salva no banco de dados
        public byte[] PreparaImagem(IFormFile imagem)
        {
            using (var memoryStream = new MemoryStream())
            {
                imagem.CopyTo(memoryStream);
                byte[] dadosImagem = memoryStream.ToArray();
                return dadosImagem;
            }
        }
    }
}
