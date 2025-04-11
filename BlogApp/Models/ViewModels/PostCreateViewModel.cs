using System.ComponentModel.DataAnnotations;
using BlogApp.Dtos.CategoryDtos;
using Microsoft.AspNetCore.Http; // IFormFile için gerekli

namespace BlogApp.Models.ViewModels
{
    public class PostCreateViewModel
    {
        public int PostId { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [Display(Name = "Başlık")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; } = string.Empty;        

        [Required(ErrorMessage = "İçerik alanı zorunludur.")]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = string.Empty;        

        [Display(Name = "Resim Yükle")]
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    }
}
