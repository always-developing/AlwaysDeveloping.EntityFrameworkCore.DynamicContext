using DynamicContext.Model;
using Microsoft.EntityFrameworkCore;

namespace DynamicContext.ConsoleSeed
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blog { get; set; }
    }
}