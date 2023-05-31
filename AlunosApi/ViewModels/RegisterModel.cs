using System.ComponentModel.DataAnnotations;

namespace AlunosApi.ViewModels {
    public class RegisterModel {
        [Required(ErrorMessage = "Email obrigatório")]
        [EmailAddress(ErrorMessage = "Formato inválido")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Senha obrigatória")]
        [StringLength(20, ErrorMessage = "A senha deve ter no mínimo 10 e no máximo 20 caracteres", MinimumLength = 10)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirma senha")]
        [Compare("Password", ErrorMessage = "Senhas não conferem")]
        public string ConfirmPassword { get; set; }
    }
}
