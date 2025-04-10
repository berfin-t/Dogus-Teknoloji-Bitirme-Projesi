using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Dtos.CommentDtos;
using BlogApp.Dtos.PostDtos;
using BlogApp.Dtos.UserDtos;
using BlogApp.Entities;
using BlogApp.Models.ViewModels;
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
        private readonly ICommentRepository _commentRepository;

        public PostsController(ILogger<PostsController> logger, IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }


        public async Task<IActionResult> PostDetail(int id)
        {
            var postDto = await _postRepository.Posts
                                .FirstOrDefaultAsync(p => p.Id == id);

            if (postDto == null)
                return NotFound();

            return View(postDto);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _postRepository.CreatePostAsync(
                    new PostDto
                    {
                        Title = model.Title,
                        Content = model.Content,
                        UserId = int.Parse(userId ?? ""),
                        CreatedDate = DateTime.Now,
                        Image = "1.jpg",
                        IsActive = false
                    }
                );
                return RedirectToAction("Index");
            }
            return View(model);
        }


    }
}
