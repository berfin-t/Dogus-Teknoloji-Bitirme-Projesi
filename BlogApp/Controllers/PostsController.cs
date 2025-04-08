using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Dtos.CommentDtos;
using BlogApp.Dtos.PostDtos;
using BlogApp.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IPostRepository _postRepository;

        public PostsController(ILogger<PostsController> logger, IPostRepository postRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
        }

        public async Task<IActionResult> PostDetail(int id)
        {
            var postDto = await _postRepository.Posts
                                .FirstOrDefaultAsync(p => p.Id == id);

            if (postDto == null)
                return NotFound();

            return View(postDto);
        }

        //[Authorize]
        //public async Task<IActionResult> Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> Create(PostCreateViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //        var post = new PostDto
        //        {
        //            Title = model.Title,
        //            Content = model.Content,
        //            Url = model.Url,
        //            UserId = int.Parse(userId ?? ""),
        //            CreatedDate = DateTime.Now,
        //            Image = "1.jpg",
        //            IsActive = false
        //        };

        //        // Asenkron veritabanı işlemi
        //        await _postRrepository.CreatePostAsync(post);

        //        return RedirectToAction("Index");
        //    }
        //    return View(model);
        //}
    }
}
