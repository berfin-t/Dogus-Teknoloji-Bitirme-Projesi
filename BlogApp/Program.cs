using BlogApp.Data.BlogAppDbContext;
using BlogApp.Data.DataSeeder;
using BlogApp.Data.Repositories.Implementations.EfCore;
using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Mapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(BlogAppAutoMapperProfile).Assembly); 

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("connection")
    );    
});

builder.Services.AddScoped<IPostRepository, EfCorePostRepository>();
builder.Services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
builder.Services.AddScoped<ICommentRepository, EfCoreCommentRepository>();
builder.Services.AddScoped<IUserRepository, EfCoreUserRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.AccessDeniedPath = "/Users/AccessDenied";
    options.LoginPath = "/Users/Login";
    options.LogoutPath = "/Users/Logout";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

SeedData.CreateSeedData(app);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
