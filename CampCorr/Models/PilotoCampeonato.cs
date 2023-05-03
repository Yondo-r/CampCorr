using System.ComponentModel.DataAnnotations;

namespace CampCorr.Models
{
    public class PilotoCampeonato
    {
        [Key]
        public int Id { get; set; }
        public int PilotoId { get; set; }
        public int CampeonatoId { get; set; }
    }
}
