using System.ComponentModel.DataAnnotations;
namespace CampCorr.Models
{
    public class Circuito
    {
        [Key]
        public int CircuitoId { get; set; }
        [Display(Name="Nome do Circuito")]
        public string Nome { get; set; }
        [Display(Name ="Endereço do Circuito")]
        public string Endereço { get; set; }
        public string Tipo { get; set; }
    }
}
