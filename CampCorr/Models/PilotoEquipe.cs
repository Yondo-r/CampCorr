using System.ComponentModel.DataAnnotations;

namespace CampCorr.Models
{
    public class PilotoEquipe
    {
        [Key]
        public int Id { get; set; }
        public int PilotoId { get; set; }

        public int EquipeId { get; set; }
    }
}
