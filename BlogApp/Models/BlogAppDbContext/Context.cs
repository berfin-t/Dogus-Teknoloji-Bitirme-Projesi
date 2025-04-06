using BlogApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlogApp.Models.BlogAppDbContext
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)  
                .WithMany(p => p.Comments)  
                .HasForeignKey(c => c.PostId)  
                .OnDelete(DeleteBehavior.NoAction);  
        }
    }
}
