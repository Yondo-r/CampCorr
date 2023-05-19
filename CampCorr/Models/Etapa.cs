using System.ComponentModel.DataAnnotations;

namespace CampCorr.Models
{
    public class Etapa
    {
        [Key]
        public int EtapaId { get; set; }
        [Display(Name ="Nome do traçado")]
        public string Traçado { get; set; }
        [Required(ErrorMessage = "Informe o dia e a hora da corrida")]
        [Display(Name = "Data e hora da corrida")]
        public DateTime Data { get; set; }
        [Display(Name = "Etapa")]
        public string NumeroEvento { get; set; }
        public bool Concluido { get; set; }
        public int TemporadaId { get; set; }
        public int CircuitoId { get; set; }
        public virtual Temporada Temporada { get; set; }
        public virtual Circuito Circuito { get; set; }
    }   
}
