using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Dtos.CommentDtos;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        #region Create Comment
        [HttpPost]
        public async Task<JsonResult> CreateComment(int PostId, string Text)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var userprofile = User.FindFirstValue(ClaimTypes.UserData);

            if (!int.TryParse(userIdStr, out var userId))
            {
                return Json(new { error = "Kullanıcı bilgisi alınamadı." });
            }

            var commentDto = new CommentCreateDto
            {
                PostId = PostId,
                Text = Text,
                UserId = userId,
                CreatedDate = DateTime.Now
            };

            var createdComment = await _commentRepository.CreateCommentAsync(commentDto);

            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("PostDetails", "Posts", new { id = PostId }),
                userprofile = userprofile,
                username = username,
                Text = Text,
                CreatedDate = commentDto.CreatedDate
            });

        }
        #endregion

        [Authorize]
        public async Task<IActionResult> EditComment(int commentId)
        {
            if (commentId == 0)
            {
                return NotFound();
            }

            var comment = await _commentRepository.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var commentViewModel = new CommentViewModel
            {
                Id = comment.Id,
                Text = comment.Text
            };

            return View(commentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditComment(CommentViewModel comment)
        {
            if (ModelState.IsValid)
            {
                var commentDto = new CommentDto
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    CreatedDate = DateTime.Now,
                    PostId = comment.PostId
                };

                await _commentRepository.EditCommentAsync(commentDto);
                
                return RedirectToAction("PostDetail", "Posts", new { id = comment.PostId });
            }
            return RedirectToAction("PostDetail", "Posts", new { id = comment.PostId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _commentRepository.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null)
            {
                return NotFound();
            }

            await _commentRepository.DeleteCommentAsync(commentId);

            TempData["SuccessMessage"] = "Yorum başarıyla silindi.";

            return RedirectToAction("PostDetail", "Posts", new { id = comment.PostId });
        }

    }
}
