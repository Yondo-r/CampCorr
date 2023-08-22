using System.ComponentModel.DataAnnotations;

namespace CampCorr.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o nome")]
        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Senha Antiga")]
        public string SenhaAntiga { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Senha Nova")]
        public string SenhaNova { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Repita a senha novamente")]
        public string RepeteSenhaNova { get; set; }
        public string ReturnUrl { get; set; }
        public string Area { get; set; }
    }
}
