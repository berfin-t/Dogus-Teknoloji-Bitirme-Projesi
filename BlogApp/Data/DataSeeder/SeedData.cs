using BlogApp.Data.BlogAppDbContext;
using BlogApp.Entities;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BlogApp.Data.DataSeeder
{
    public static class SeedData
    {
        public static void CreateSeedData(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<Context>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            // Kategori verileri
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Web Programlama" },
                    new Category { Name = "Backend" },
                    new Category { Name = "Frontend" },
                    new Category { Name = "Oyun Geliştirme" },
                    new Category { Name = "Mobil" }
                );
                context.SaveChanges();
            }

            // Kullanıcı verileri
            if (!context.Users.Any())
            {
                var passwordHasher = new PasswordHasher<User>(); 

                var userFaker = new Faker<User>("tr")
                    .RuleFor(u => u.UserName, f => f.Internet.UserName())
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.Email, f => f.Internet.Email())
                    .RuleFor(u => u.UserProfile, f => f.Image.PicsumUrl())
                    .RuleFor(u => u.IsDeleted, false);

                var users = userFaker.Generate(50);

                foreach (var user in users)
                {
                    user.Password = passwordHasher.HashPassword(user, "12345678"); 
                }

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            if (!context.Posts.Any())
            {
                var userIds = context.Users.Select(u => u.Id).ToList();
                var categoryIds = context.Categories.Select(c => c.Id).ToList();

                var postFaker = new Faker<Post>("tr")
                    .RuleFor(p => p.Title, f => f.Lorem.Sentence(5))
                    .RuleFor(p => p.Content, f => f.Lorem.Paragraphs(2))
                    .RuleFor(p => p.Description, f => f.Lorem.Sentence(150))
                    .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
                    .RuleFor(p => p.CreatedDate, f => f.Date.Past(1))
                    .RuleFor(p => p.IsActive, true)
                    .RuleFor(p => p.UserId, f => f.PickRandom(userIds))
                    .RuleFor(p => p.CategoryId, f => f.PickRandom(categoryIds))
                    .RuleFor(p => p.Comments, f =>
                        new List<Comment>
                        {
                            new Comment
                            {
                                Text = f.Lorem.Sentence(),
                                CreatedDate = f.Date.Recent(),
                                IsDeleted = false,
                                UserId = f.PickRandom(userIds)
                            },
                            new Comment
                            {
                                Text = f.Lorem.Sentence(),
                                CreatedDate = f.Date.Recent(),
                                IsDeleted = false,
                                UserId = f.PickRandom(userIds)
                            },
                            new Comment
                            {
                                Text = f.Lorem.Sentence(),
                                CreatedDate = f.Date.Recent(),
                                IsDeleted = false,
                                UserId = f.PickRandom(userIds)
                            }
                        });

                var posts = postFaker.Generate(50);
                context.Posts.AddRange(posts);
                context.SaveChanges();
            }
        }
    }
}
