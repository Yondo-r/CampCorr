using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampCorr.Models
{
    public class Piloto
    {
        [Key]
        public int PilotoId { get; set; }
        [Display(Name ="Tipo Sanguineo do Piloto")]
        public string Nome { get; set; }
        public string TipoSanguineo { get; set; }
        [Display(Name ="Peso médio do piloto")]
        [Column(TypeName = "Decimal(5,2)")]
        public decimal Peso { get; set; }
        [Display (Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime? DataNascimento { get; set; }
        public string UsuarioId { get; set; }
        [Display (Name= "Descrição do Piloto")]
        public string Descricao { get; set; }
        public string Foto { get; set; }
    }
}
