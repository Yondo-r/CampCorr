using CampCorr.Models;
using System.ComponentModel.DataAnnotations;

namespace CampCorr.ViewModels
{
    public class PilotoViewModel
    {
        

        public int PilotoId { get; set; }
        [Display(Name = "Nome de Usuário")]
        public string UserLogin { get; set; }
        [Required(ErrorMessage = "Informe seu nome")]
        [Display(Name = "Nome do Piloto")]
        [StringLength(80)]
        public string NomePiloto { get; set; }
        [Required(ErrorMessage = "Informe seu tipo samguíneo")]
        [Display(Name = "Tipo Sanguíneo")]
        public string TipoSanguineo { get; set; }
        [Display(Name = "Peso")]
        [Required(ErrorMessage = "Informe seu peso aproximado")]
        public decimal Peso { get; set; }
        [Display(Name = "Data de nascimento")]
        [Required(ErrorMessage = "Informe sua data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime? DataNascimento { get; set; } 
        public string NomeEquipe { get; set; }
        [Display(Name = "Descrição do Piloto")]
        public string DescricaoPiloto { get; set; }
        [Required(ErrorMessage = "Informe um telefone para contato")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Informe seu e-mail")]
        public string Email { get; set; }
        [Display(Name = "Foto")]
        public string Foto { get; set; }
        public List<ResultadoCorrida> ResultadosPiloto { get; set; }

        public PilotoViewModel() { }
        public PilotoViewModel(int pilotoId, string userName, string nome)
        {
            PilotoId = pilotoId;
            this.UserLogin= userName;
            this.NomePiloto = nome;
        }
    }
}
