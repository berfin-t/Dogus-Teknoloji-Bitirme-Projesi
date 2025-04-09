using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.ViewModels
{
    public class PostCreateViewModel
    {
        public int PostId { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [Display(Name = "Başlık")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        [Display(Name = "Açıklama")]
        [StringLength(250, ErrorMessage = "Açıklama en fazla 250 karakter olabilir.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik alanı zorunludur.")]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "URL alanı zorunludur.")]
        [Display(Name = "Url")]
        [Url(ErrorMessage = "Geçerli bir URL giriniz.")]
        public string Url { get; set; } = string.Empty;

        [Display(Name = "Görsel URL")]
        [Url(ErrorMessage = "Geçerli bir görsel URL giriniz.")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; } = true;
    }
}
