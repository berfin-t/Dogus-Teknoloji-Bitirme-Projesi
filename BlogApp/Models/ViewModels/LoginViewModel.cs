using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi girin.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "{0} alanı en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
