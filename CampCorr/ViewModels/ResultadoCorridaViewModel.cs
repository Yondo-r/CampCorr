using CampCorr.Models;
using System.ComponentModel.DataAnnotations;

namespace CampCorr.ViewModels
{
    public class ResultadoCorridaViewModel
    {
        [Display(Name = "Posição de chegada")]
        [Range(1,50, ErrorMessage = "O valor deve estar entre 1 e 50")]
        public int? Posicao { get; set; }
        [Display(Name = "Total de pontos ganho")]
        public int? Pontos { get; set; }
        [Display(Name = "Nome da equipe")]
        public string NomeEquipe { get; set; }
        [Display(Name = "Nome do piloto")]
        public string NomePiloto { get; set; }
        [Display(Name = "Descrição da penalidade")]
        public string DescricaoPenalidade { get; set; }
        [Display(Name = "Pontos de penalidade")]
        public int? PontosPenalidade { get; set; }
        [Display(Name = "Melhor volta da corrida")]
        public bool MelhorVolta { get; set; }
        [Display(Name = "Posição de largada")]
        public int? PosicaoLargada { get; set; }
        [Display(Name = "Tempo total da prova")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm:ss.ff}")]
        public TimeSpan? TempoTotal { get; set; }
        [Display(Name = "Tempo da melhor volta")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:mm:ss.fff}")]
        public TimeSpan? TempoMelhorVolta { get; set; }
        [Display(Name = "Numero total de voltas")]
        public int? TotalVoltas { get; set; }
        public int PilotoId { get; set; }
        public int EtapaId { get; set; }
        public int ResultadoId { get; set; }
        public int EquipeId { get; set; }
        [Display(Name = "Número de vitórias")]
        public int NumeroVitorias { get; set; }
        public List<Equipe> ListaEquipe { get; set; }
        public ResultadoCorrida Resultado { get; set; }
    }
}
