using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Dtos.CommentDtos;
using BlogApp.Dtos.UserDtos;
using BlogApp.Entities;
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
        [HttpPost]
        public async Task<JsonResult> CreateComment(int PostId, string Text)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var profile = User.FindFirstValue(ClaimTypes.UserData);

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
                UserProfile = profile,
                username = username,
                Text = Text,
                CreatedDate = commentDto.CreatedDate
            });

        }

        //[HttpPost]
        //public async Task<JsonResult> AddComment(int PostId, string Text)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var username = User.FindFirstValue(ClaimTypes.Name);
        //    var avatar = User.FindFirstValue(ClaimTypes.UserData);

        //    var entity = new CommentCreateDto
        //    {
        //        Text = Text,
        //        CreatedDate = DateTime.Now,
        //        PostId = PostId,
        //        UserId = int.Parse(userId ?? "")
        //    };

        //    await _commentRepository.CreateCommentAsync(entity); 

        //    return Json(new
        //    {
        //        username,
        //        Text,
        //        entity.CreatedDate,
        //        avatar
        //    });
        //}

        [Authorize]
        public async Task<IActionResult> EditComment(int commentId)
        {
            if (commentId == 0)
            {
                return NotFound();
            }

            // Yorumun veritabanından çekilmesi
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

        // Yorum Düzenleme (POST)
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
                    CreatedDate = DateTime.Now,
                    PostId = comment.PostId
                };

                await _commentRepository.EditCommentAsync(commentDto);

                return RedirectToAction("PostDetail", "Posts", new { id = comment.PostId });
            }

            return View(comment);
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
