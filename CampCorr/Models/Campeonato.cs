using System.ComponentModel.DataAnnotations;

namespace CampCorr.Models
{
    public class Campeonato
    {
        [Key]
        public int CampeonatoId { get; set; }
        public string UserId { get; set; }
        public byte[] Logo { get; set; }

        public List<Temporada> Temporadas{ get; set; }
        public List<Equipe> Equipes { get; set; }
    }
}
