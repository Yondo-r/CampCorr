using System.ComponentModel.DataAnnotations;

namespace CampCorr.Models
{
    public class Equipe
    {
        [Key]
        public int EquipeId { get; set; }
        public string Nome { get; set; }
        public int CampeonatoId { get; set; }
        public byte[] Emblema { get; set; }
    }
}
