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
                var isUserDto = await _userRepository.Users
                    .FirstOrDefaultAsync(x => x.Email == model.Email);

                if (isUserDto != null)
                {
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
                    new Claim(ClaimTypes.GivenName, isUser.FirstName ?? "")
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

            var newUserDto = new UserCreateDto
            {
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = hashedPassword,
                UserProfile = "p1.jpg"
            };

            await _userRepository.CreateUserAsync(newUserDto);

            TempData["SuccessMessage"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }
    }
}
