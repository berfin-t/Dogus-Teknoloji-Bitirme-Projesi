using Microsoft.Extensions.Hosting;

namespace BlogApp.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false; 

        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
