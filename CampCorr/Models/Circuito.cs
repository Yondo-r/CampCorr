using System.ComponentModel.DataAnnotations;
namespace CampCorr.Models
{
    public class Circuito
    {
        [Key]
        public int CircuitoId { get; set; }
        [Display(Name="Nome do Kartodromo")]
        public string Nome { get; set; }
        [Display(Name ="Endereço do Kartodromo")]
        public string Endereço { get; set; }
    }
}
