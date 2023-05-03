using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampCorr.Models
{
    public class Temporada
    {
        public int TemporadaId { get; set; }
        public int AnoTemporada { get; set; } = DateTime.Now.Year;
        [Required(ErrorMessage = "Informe quantas etapas terá sua temporada")]
        [Display(Name = "Quantidade de etapas")]
        [Column(TypeName = "Int")]
        public int QuantidadeEtapas { get; set; }
        public int CampeonatoId { get; set; }
        public int RegulamentoId { get; set; }
        public bool Concluida { get; set; }
        public virtual Campeonato Campeonato { get; set; }
        public virtual Regulamento Regulamento { get; set; }
        //TODO Remover listas e adicionar ViewModel para trabalhar essas listas
        public List<Etapa> Etapas { get; set; }
        public List<Piloto> Pilotos { get; set; }
        public List<Equipe> Equipes { get; set; }
    }
}
