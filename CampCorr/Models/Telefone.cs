using System.ComponentModel.DataAnnotations;

namespace CampCorr.Models
{
    public class Telefone
    {
        [Key]
        public int TelefoneId { get; set; }
        public int UsuarioId { get; set; }
        public string Numero { get; set; }
    }
}
