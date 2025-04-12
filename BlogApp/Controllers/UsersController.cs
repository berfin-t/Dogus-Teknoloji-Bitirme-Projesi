using Microsoft.AspNetCore.Identity;
using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BlogApp.Dtos.UserDtos;
using BlogApp.Entities;
using Microsoft.AspNetCore.Authorization;
using BlogApp.Dtos.PostDtos;

namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public UsersController(ILogger<UsersController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>(); 
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUserDto = await _userRepository.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

                if (isUserDto != null)
                {
                    if (isUserDto.IsDeleted)
                    {
                        ModelState.AddModelError("", "Bu kullanıcı silinmiş. Giriş yapamazsınız.");
                        return View(model);
                    }
                    var isUser = new User
                    {
                        Id = isUserDto.Id,
                        UserName = isUserDto.UserName,
                        FirstName = isUserDto.FirstName,
                        LastName = isUserDto.LastName,
                        Email = isUserDto.Email,
                        Password = isUserDto.Password,  
                        UserProfile = isUserDto.UserProfile
                    };

                    if (_passwordHasher.VerifyHashedPassword(isUser, isUser.Password, model.Password) != PasswordVerificationResult.Failed)
                    {
                        var userClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, isUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, isUser.UserName ?? ""),
                    new Claim(ClaimTypes.GivenName, isUser.FirstName ?? ""),
                    new Claim(ClaimTypes.UserData, isUser.UserProfile ?? "")
                };

                        if (isUser.Email == "berfintek@mail.com")
                        {
                            userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                        }

                        var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties { IsPersistent = true };

                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı");
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await _userRepository.Users
                .FirstOrDefaultAsync(x => x.UserName == model.Username || x.Email == model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya email zaten kullanımda.");
                return View(model);
            }

            var hashedPassword = _passwordHasher.HashPassword(new User(), model.Password);
            string imageUrl = "https://localhost:7174/images/default.jpg";
            if (model.UserProfile != null)
            {
                var fileName = Path.GetFileName(model.UserProfile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.UserProfile.CopyToAsync(stream);
                }

                imageUrl = "https://localhost:7174/images/" + fileName;
            }

            var newUserDto = new UserCreateDto
            {
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = hashedPassword,
                UserProfile = imageUrl
            };

            await _userRepository.CreateUserAsync(newUserDto);

            TempData["SuccessMessage"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        [HttpGet("Users/Detail/{userName}")]
        public async Task<IActionResult> Detail(string userName)
        {
            var user = await _userRepository.GetUserWithNameAsync(userName);

            if (user == null)
            {
                return NotFound();
            }

            return View(user); 
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _userRepository.Users.FirstOrDefault(i => i.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(new RegisterViewModel
            {
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                ImagePath = user.UserProfile,

            });
        }                 

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = _passwordHasher.HashPassword(new User(), model.Password);

                string imageUrl = "https://localhost:7174/images/default.jpg";
                var dto = new UserDto
                {
                    Id = model.Id,
                    UserName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = hashedPassword
                };
                if (model.UserProfile != null)
                {
                    var fileName = Path.GetFileName(model.UserProfile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UserProfile.CopyToAsync(stream);
                    }

                    dto.UserProfile = "https://localhost:7174/images/" + model.UserProfile.FileName;
                }
                else
                {
                    dto.UserProfile = model.ImagePath;
                }
                
                await _userRepository.EditUser(dto);

                var updatedUser = await _userRepository.Users
            .FirstOrDefaultAsync(x => x.UserName == model.Username || x.Email == model.Email);

                if (updatedUser == null)
                {
                    return NotFound();
                }

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, updatedUser.Id.ToString()),
            new Claim(ClaimTypes.Name, updatedUser.UserName ?? ""),
            new Claim(ClaimTypes.GivenName, updatedUser.FirstName ?? ""),
            new Claim(ClaimTypes.UserData, updatedUser.UserProfile ?? "")
        };

                if (updatedUser.Email == "berfintek@mail.com")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "admin"));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignOutAsync(); 
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Detail", "Users", new { userName = updatedUser.UserName });
            }
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _userRepository.Users.FirstOrDefaultAsync(c => c.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            await _userRepository.DeleteUserAsync(userId);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi.";
            return RedirectToAction("Index", "Home");
        }
    }
}
