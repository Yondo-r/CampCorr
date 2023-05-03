using System.ComponentModel.DataAnnotations;
namespace CampCorr.Models
{
    public class ResultadoCorrida
    {
        [Key]
        public int ResultadoId { get; set; }
        public int EtapaId { get; set; }
        public int? Posicao { get; set; }
        [Display(Name ="Pontuação ganha")]
        public int? Pontos { get; set; }
        public int EquipeId { get; set; }
        public int PilotoId { get; set; }
        [Display(Name ="Descrição da penalidade aplicada")]
        public string DescricaoPenalidade { get; set; }
        public int? PontosPenalidade { get; set; }
        [Display(Name = "Melhor volta do piloto")]
        public bool MelhorVolta { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:mm:ss.ff}")]
        public DateTime? TempoMelhorVolta { get; set; }
        [Display(Name ="Posição em que o piloto largou")]
        public int? PosicaoLargada { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm:ss.ff}")]
        public DateTime? TempoTotal { get; set; }
    }
}
