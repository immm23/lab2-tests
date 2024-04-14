using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace OOPLab1.ViewModels
{
    #region Attribues
    [ExcludeFromCodeCoverage]
    #endregion
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Ім'я користувача")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Паролі різні")]
        [Display(Name = "Повтор Паролю")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
    }
}
