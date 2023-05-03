using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampCorr.Models
{
    public class EquipeTemporada
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Temporada")]
        public int TemporadaId { get; set; }
        [ForeignKey("Equipe")]
        public int EquipeId { get; set; }
    }
}
