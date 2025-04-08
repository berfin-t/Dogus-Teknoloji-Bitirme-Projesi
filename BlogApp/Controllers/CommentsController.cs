using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Dtos.CommentDtos;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ILogger<CommentsController> _logger;
        private readonly ICommentRepository _commentRepository;
        public CommentsController(ILogger<CommentsController> logger, ICommentRepository commentRepository)
        {
            _logger = logger;
            _commentRepository = commentRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> CreateComment(int PostId, string Text)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var username = User.FindFirstValue(ClaimTypes.Name);
                var avatar = User.FindFirstValue(ClaimTypes.UserData);

                if (string.IsNullOrEmpty(userId))
                    throw new InvalidOperationException("Kullanıcı kimliği eksik.");

                var entity = new CommentCreateDto
                {
                    Text = Text,
                    CreatedDate = DateTime.Now,
                    PostId = PostId,
                    UserId = int.Parse(userId ?? "")
                };

                await _commentRepository.CreateCommentAsync(entity);

                return Json(new
                {
                    username,
                    Text,
                    entity.CreatedDate,
                    avatar
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum oluşturulurken hata oluştu");
                return Json(new { error = "Yorum oluşturulurken hata oluştu" });
            }
        }

        [Authorize]
        public async Task<IActionResult> EditComment(int commentId, string text)
        {
            if(commentId==null)
            {
                return NotFound();
            }

            var comment = _commentRepository.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return NotFound();
            }

            return View(new CommentViewModel
            {
                Id = comment.Id,
                Text = comment.Text
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditComment(CommentViewModel comment)
        {
            if (ModelState.IsValid)
            {
                var commentDto = new CommentDto
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    CreatedDate = DateTime.Now
                };
                await _commentRepository.EditPost(commentDto);
                return RedirectToAction("Index", "Posts");
            }
            return View(comment);
        }
    }
}
