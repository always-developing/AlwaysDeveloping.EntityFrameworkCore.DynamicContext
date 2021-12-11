using DynamicContext.Model;
using Microsoft.EntityFrameworkCore;

namespace DynamicContext.Console
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public Action<DbContextOptionsBuilder>? DynamicContextOptionsBuilder { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Blog> Blog { get; set; }

    }
}