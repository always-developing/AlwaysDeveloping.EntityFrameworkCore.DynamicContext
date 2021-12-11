using DynamicContext.ConsoleSeed;
using DynamicContext.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var seedCount = 100000;

IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) => services
        .AddDbContext<BlogContext>(x => x.UseSqlite($"Data Source={Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\BlogDatabase.db"))
    ).Build();

var context = host.Services.GetService<BlogContext>();
context.Database.EnsureCreated();


var rnd = new Random();

for (int b = 0; b < seedCount; b++)
{
    var blogId = Guid.NewGuid();
    var postCount = rnd.Next(1, 3);

    var posts = new List<Post>(postCount);

    for(int p = 0; p < postCount; p++)
    {
        var postId = Guid.NewGuid();

        posts.Add(new Post
        {
            Id = postId,
            BlogId = blogId,
            Content = $"Content: {postId}",
            Title = $"Title: {postId}",
            WordCount = rnd.Next(100, 1000)
        });
    }

    context.Blog.Add(new Blog
    {
        Id = blogId,
        Title = $"Title: {blogId}",
        Owner = $"Owner: {blogId}",
        Posts = posts,
        Url = $"http://www.{blogId}.com"
    });
}

context.SaveChanges();