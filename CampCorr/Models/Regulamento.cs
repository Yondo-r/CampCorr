using System.ComponentModel.DataAnnotations;
namespace CampCorr.Models
{
    public class Regulamento
    {
        [Key]
        public int RegulamentoId { get; set; }
        [Display (Name ="Descricao do Regulamento")]
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
