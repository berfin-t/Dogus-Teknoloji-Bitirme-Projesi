using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Dtos.CommentDtos;
using BlogApp.Dtos.PostDtos;
using BlogApp.Entities;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _postRepository.Posts.FirstOrDefault(i => i.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(new PostCreateViewModel
            {
                PostId = post.Id,
                Title = post.Title,
                Content = post.Content,
                IsActive = post.IsActive,
                ImagePath = post.Image,
                CategoryId = post.CategoryId,
                Categories = _categoryRepository.Categories.ToList(),

            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = "https://localhost:7174/images/default.jpg";
                var dto = new PostDto
                {
                    Id = model.PostId,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedDate = DateTime.Now,
                    UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? ""),
                    CategoryId = model.CategoryId,
                };
                if (model.Image != null)
                {
                    var fileName = Path.GetFileName(model.Image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    dto.Image = "https://localhost:7174/images/" + model.Image.FileName;
                }
                else
                {
                    dto.Image = model.ImagePath; 
                }
                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    dto.IsActive = model.IsActive;
                }
                await _postRepository.EditPost(dto);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

    }
}
