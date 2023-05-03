using System.ComponentModel.DataAnnotations;

namespace CampCorr.ViewModels
{
    public class LoginCampeonatoViewModel
    {
        [Required(ErrorMessage = "Informe o nome")]
        [Display(Name = "Campeonato")]
        public string NomeCampeonato { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }
        public string ReturnUrl { get; set; }
    }
}
