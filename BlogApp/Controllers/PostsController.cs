using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Dtos.CommentDtos;
using BlogApp.Dtos.PostDtos;
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
        private readonly ICategoryRepository _categoryRepository;

        public PostsController(ILogger<PostsController> logger, IPostRepository postRepository, ICommentRepository commentRepository, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _categoryRepository = categoryRepository;
        }


        public async Task<IActionResult> PostDetail(int id)
        {
            var postDto = await _postRepository.Posts
                                .FirstOrDefaultAsync(p => p.Id == id);
            if (postDto == null)
                return NotFound();

            postDto.CommentDtos = postDto.CommentDtos?.Where(c => !c.IsDeleted).ToList()
                                  ?? new List<CommentDto>();

            if (postDto == null)
                return NotFound();

            return View(postDto);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var model = new PostCreateViewModel
            {
                Categories = _categoryRepository.Categories.ToList() 
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                model.Categories = _categoryRepository.Categories.ToList();

                string imageUrl = "https://localhost:7174/images/default.jpg";  

                if (model.Image != null)  
                {
                    var fileName = Path.GetFileName(model.Image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    imageUrl = "https://localhost:7174/images/" + fileName;
                }
                
                model.Categories = _categoryRepository.Categories.ToList(); 

                var postDto = new PostDto
                {
                    Title = model.Title,
                    Content = model.Content,
                    UserId = int.Parse(userId ?? ""),
                    CreatedDate = DateTime.Now,
                    Image = imageUrl,
                    IsActive = model.IsActive,
                    CategoryId = model.CategoryId
                };

                await _postRepository.CreatePostAsync(postDto);

                return RedirectToAction("Index", "Home");
            }

            model.Categories = _categoryRepository.Categories.ToList();
            return View(model);
        }

    }
}
