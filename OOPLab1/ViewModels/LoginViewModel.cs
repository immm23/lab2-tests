using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace OOPLab1.ViewModels
{
    #region Attribues
    [ExcludeFromCodeCoverage]
    #endregion
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Ім'я користувача")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль ")]
        public string Password { get; set; }

        [Display(Name = "Запам`ятати?")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
