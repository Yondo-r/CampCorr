using CampCorr.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampCorr.ViewModels
{
    public class CampeonatoViewModel
    {
        public int IdCampeonato { get; set; }
        [Required(ErrorMessage = "Informe o nome")]
        [Display(Name = "Nome do campeonato")]
        [StringLength(50)]
        public string NomeCampeonato { get; set; }
        public string UserId { get; set; }
        public string Logo { get; set; }
        public string Senha { get; set; }
        public int AnoTemporada { get; set; } = DateTime.Now.Year;
        [Required(ErrorMessage = "Informe quantas etapas terá sua temporada")]
        [Display(Name = "Quantidade de etapas")]
        [Column(TypeName = "Int")]
        public int QuantidadeEtapas { get; set; }
        [Display(Name = "Regras")]
        public string Regulamento { get; set; }
        [Display(Name = "Nome do traçado")]
        public string Traçado { get; set; }
        [Required(ErrorMessage = "Informe o dia e a hora da corrida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0:dd/MM hh:mm")]
        [Display(Name = "Data e hora da corrida")]
        public DateTime Data { get; set; }
        [Display(Name = "Etapa")]
        public string NumeroEvento { get; set; }
        [Display(Name = "Nome do Circuito")]
        public string NomeCircuito { get; set; }
        public List<Circuito> Circuitos { get; set; }
        public List<Temporada> Temporadas { get; set; }
        public List<Etapa> Etapas { get; set; }

        public CampeonatoViewModel() { }
        public CampeonatoViewModel(int campeonatoId, string userId, string userName, string logo)
        {
            this.IdCampeonato = campeonatoId;
            this.UserId = userId;
            this.NomeCampeonato = userName;
            this.Logo = logo;
        }

    }
}
