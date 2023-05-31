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
        [Display(Name = "Pontos de penalidade")]
        public int? PontosPenalidade { get; set; }
        [Display(Name = "Melhor volta da prova")]
        public bool MelhorVolta { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:mm:ss.fff}")]
        [Display(Name = "Tempo da melhor volta")]
        public TimeSpan? TempoMelhorVolta { get; set; }
        [Display(Name = "Numero total de voltas")]
        public int? TotalVoltas { get; set; }
        [Display(Name ="Posição de largada")]
        public int? PosicaoLargada { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm:ss.fff}")]
        [Display(Name = "Tempo total da prova")]
        public TimeSpan? TempoTotal { get; set; }
        public bool Advertencia { get; set; }
        public byte[] ImagemResultadoCorrida { get; set; }
    }
}
