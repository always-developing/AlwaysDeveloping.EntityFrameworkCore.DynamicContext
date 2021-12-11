using AlwaysDeveloping.EntityFrameworkCore.DynamicContext;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace DynamicContext.Benchmark
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        private readonly IHost host;

        public Benchmarks()
        {
            host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => services
                    .AddDynamicContext<BlogContext>(x => x.UseSqlite($"Data Source={Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\BlogDatabase.db"))
                ).Build();
        }

        //[Benchmark(Baseline = true)]
        //public void DirectDbSet()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<BlogContext>();
        //    var blogs = context.Blog.AsNoTracking().ToList();
        //}

        //[Benchmark]
        //public void GenericDbSet()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<BlogContext>();
        //    var blogs = context.Set<Blog>().AsNoTracking().ToList();
        //}

        //[Benchmark]
        //public void GenericDbSetRaw()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<BlogContext>();
        //    var blogs = context.Set<Blog>().FromSqlRaw("SELECT * FROM Blog").AsNoTracking().ToList();
        //}

        //[Benchmark]
        //public void GenericDbSetProj()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<BlogContext>();
        //    var blogs = context.Set<Blog>().Select(x => new BlogUrl { BlogId = x.Id, Url = x.Url }).AsNoTracking().ToList();
        //}

        //[Benchmark]
        //public void GenericDbSetRawProj()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<BlogContext>();
        //    var blogs = context.Set<Blog>().FromSqlRaw("SELECT Id, Url FROM Blog").Select(x => new BlogUrl { BlogId = x.Id, Url = x.Url }).AsNoTracking().ToList();
        //}

        //[Benchmark]
        //public void DynamicDbSet()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<DynamicContext<BlogContext>>();
        //    var blogs = context.Set<BlogUrl>().FromSqlRaw("SELECT Id as BlogId, Url FROM Blog").AsNoTracking().ToList();
        //}

        //[Benchmark]
        //public void DynamicDbSetAnon()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<DynamicContext<BlogContext>>();
        //    var anonBlogUrl = new { BlogId = 0, Url = "" };
        //    var blogs = context.Set(anonBlogUrl).FromSqlRaw("SELECT Id as BlogId, Url FROM Blog").AsNoTracking().ToList();
        //}

        //-------------------------------------------------------

        //[Benchmark(Baseline = true)]
        //public void DirectDbSet()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<BlogContext>();
        //    var blogIds = context.Set<Blog>().AsNoTracking().Select(x => x.Id).ToList();
        //}

        //[Benchmark]
        //public void ValueSetSelect()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<DynamicContext<BlogContext>>();
        //    var blogIds = context.ValueSet<Guid>().FromSqlRaw("SELECT Id as Value FROM Blog").AsNoTracking().Select(x => x.Value).ToList();
        //}

        //[Benchmark]
        //public void ValueSetToSimple()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<DynamicContext<BlogContext>>();
        //    var blogIds = context.ValueSet<Guid>().FromSqlRaw("SELECT Id as Value FROM Blog").AsNoTracking().ToSimple().ToList();
        //}

        //-------------------------------------------------------

        //[Benchmark(Baseline = true)]
        //public void DirectDbSetString()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<BlogContext>();
        //    var urls = context.Set<Blog>().FromSqlRaw("SELECT Url FROM Blog").AsNoTracking().Select(x => x.Url).ToList();
        //}

        //[Benchmark]
        //public void StringSetSelect()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<DynamicContext<BlogContext>>();
        //    var urls = context.StringSet<string>().FromSqlRaw("SELECT Url as Value FROM Blog").AsNoTracking().Select(x => x.Value).ToList();
        //}

        //[Benchmark]
        //public void StringSetToSimple()
        //{
        //    var context = host.Services.CreateScope().ServiceProvider.GetService<DynamicContext<BlogContext>>();
        //    var urls = context.StringSet<string>().FromSqlRaw("SELECT Url as Value FROM Blog").AsNoTracking().ToSimple().ToList();
        //}
    }
}
