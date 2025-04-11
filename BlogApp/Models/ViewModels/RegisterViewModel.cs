using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        [Display(Name = "Kullanıcı Adı")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Ad gereklidir.")]
        [Display(Name = "Ad")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Soyad gereklidir.")]
        [Display(Name = "Soyad")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "E-posta gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta formatı.")]
        [Display(Name = "E-posta")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Parola gereklidir.")]
        [StringLength(10, ErrorMessage = "{0} alanı en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Parola doğrulaması gereklidir.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Parolanız eşleşmiyor.")]
        [Display(Name = "Parola Doğrulama")]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Resim Yükle")]
        public IFormFile? UserProfile { get; set; }
        public string? ImagePath { get; set; }
    }
}
