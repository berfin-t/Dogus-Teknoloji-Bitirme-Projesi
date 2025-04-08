using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Metin alanı zorunludur.")]
        [Display(Name = "Metin")]
        [StringLength(1000, ErrorMessage = "Metin en fazla 1000 karakter olmalıdır.")]
        public string? Text { get; set; }

    }
}
