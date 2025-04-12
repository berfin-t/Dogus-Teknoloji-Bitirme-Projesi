using System.Diagnostics;
using AutoMapper;
using BlogApp.Data.BlogAppDbContext;
using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Dtos.PostDtos;
using BlogApp.Models;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IPostRepository postRepository, IMapper mapper)
        {
            _logger = logger;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string selectedCategory = null, int pageNumber = 1, int pageSize = 6)
        {
            var query = _postRepository.Posts;

            if (!string.IsNullOrWhiteSpace(selectedCategory))
            {
                query = query.Where(p => p.CategoryDto.Name == selectedCategory);
            }

            var totalPosts = await query.CountAsync();

            var posts = await query
                .OrderByDescending(p => p.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var allCategories = await _postRepository.Posts
    .Select(p => p.CategoryDto.Name)
    .Distinct()
    .ToListAsync();

            var model = new PostViewModel
            {
                Posts = posts,
                TotalPosts = totalPosts,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Categories = allCategories,
                SelectedCategory = selectedCategory
            };

            return View(model);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
