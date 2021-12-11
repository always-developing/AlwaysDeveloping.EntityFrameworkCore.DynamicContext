using DynamicContext.Console;
using DynamicContext.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) => services
        .AddDbContext<BlogContext>(x => x.UseSqlite($"Data Source={Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\BlogDatabase.db"))
    ).Build();

// Get the first id using normal db context
var context = host.Services.GetService<BlogContext>();
Console.WriteLine(context.Set<Blog>().AsNoTracking().First().Id);

// Get the first id using a dynamic context and dbset
using var dynContext = new DynamicContext<BlogUrl>();
Console.WriteLine(dynContext.Set<BlogUrl>().FromSqlRaw("SELECT Id as BlogId, Url FROM Blog").AsNoTracking().First().BlogId);

// Get the first Id using dyanmic and anonymous entity
var anon = new { BlogId = Guid.Empty, Url = "" };
Console.WriteLine(CallWithAnon(anon).BlogId);

// Using the wrappper class
using var dynContext1 = new DynamicContext<SimpleType<Guid>>();
Console.WriteLine(dynContext1.Set<SimpleType<Guid>>().FromSqlRaw("SELECT Id as Value FROM Blog").AsNoTracking().First().Value);

// Project into just a list of Ids
var blogs2 = context.Set<Blog>()
    .AsNoTracking()
    .Select(x => x.Id).ToList();

Console.ReadKey();



static T CallWithAnon<T>(T example) where T: class
{
    using var dynContext = new DynamicContext<T>();
    return dynContext.Set<T>().FromSqlRaw("SELECT Id as BlogId, Url FROM Blog").AsNoTracking().First();
}