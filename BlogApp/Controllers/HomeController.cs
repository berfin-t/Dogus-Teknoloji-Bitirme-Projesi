using System.Diagnostics;
using AutoMapper;
using Azure;
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
        private readonly IPostRepository _postRrepository;

        public HomeController(ILogger<HomeController> logger, IPostRepository postRrepository)
        {
            _logger = logger;
            _postRrepository = postRrepository;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 6)
        {
            var totalPosts = await _postRrepository.Posts.CountAsync();

            var posts = await _postRrepository.Posts
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)  
                .ToListAsync();

            var model = new PostViewModel
            {
                Posts = posts,
                TotalPosts = totalPosts,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var post = await _postRrepository.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) return NotFound();

            return View(post);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
