# DynamicContext

Provides support for executing raw SQL queries against a Entity Framework Core DbContext without a DbSet for the entity. Itt also provides support to return a result set of single types (int, bool, Guid, string etc).

## Installing AlwaysDeveloping.EntityFrameworkCore.DynamicContext

AlwaysDeveloping.EntityFrameworkCore.DynamicContext is available on NuGet via:
    
    Install-Package AlwaysDeveloping.EntityFrameworkCore.DynamicContext
    
Or via the .NET CLI:
    
    dotnet add package AlwaysDeveloping.EntityFrameworkCore.DynamicContext

Or via NuGet Packager Manager in Visual Studio    

Direct link to AlwaysDeveloping.EntityFrameworkCore.DynamicContext on NuGet: [https://www.nuget.org/packages/AlwaysDeveloping.EntityFrameworkCore.DynamicContext/](https://www.nuget.org/packages/AlwaysDeveloping.EntityFrameworkCore.DynamicContext/) 

## How to use AlwaysDeveloping.EntityFrameworkCore.DynamicContext
### Configuring the dependency injection container

When using Entity Framework Core, when adding the DbContext to the DI container, use `AddDynamicContext instead of AddContext`

```c#
 host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) => services
        .AddDynamicContext<BlogContext>(x => x.UseSqlite($"Data Source={Environment
            .GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\BlogDatabase.db"))
    ).Build();
```

### Using DynamicContext\<T\>

Inject DynamicContext\<T\> (where T is the underlying DbContext) into the relevant class.

Using the DynamicContext to retrieve data into an `entity (or list of entities)`, without having a DbSet\<\>:
```c#
// Using the original underlying dbContext
// DbSet<Blog> must exists on the context
var blogs = dynContext.Context.Set<Blog>().AsNoTracking().ToList();

// Retrieving data dynamically
// DbSet<BlogUrl> does NOT need to exist on the context
var blogs = dynContext.Set<BlogUrl>()
    .FromSqlRaw("SELECT Id as BlogId, Url FROM Blog")
    .AsNoTracking().ToList();

// Retrieving data using anonymous object as a 'template' for the result
var anonBlogUrl = new { BlogId = 0, Url = "" };
var blogs = dynContext.Set(anonBlogUrl)
    .FromSqlRaw("SELECT Id as BlogId, Url FROM Blog")
    .AsNoTracking().ToList();
```

Using the DynamicContext to retrieve data into a `simple type (or list of simple types)` - simple types being value types (int, bool etc), Guid and strings:
```c#
// Retrieving Guid dynamically
// DbSet<Guid> does NOT exist on the context
var blogIds = dynContext.ValueSet<Guid>()
    .FromSqlRaw("SELECT Id as Value FROM Blog")
    .AsNoTracking().ToSimple().ToList();

// Retrieving string dynamically
// DbSet<string> does NOT exist on the context
var urls = context.StringSet<string>()
    .FromSqlRaw("SELECT Url as Value FROM Blog")
    .AsNoTracking().ToSimple().ToList();
```

## Benchmarks
See [this blog post](https://www.alwaysdeveloping.net/p/11-2020-dynamic-context/) for some simple benchmarks on comparing DbContext to DynamicContext (TLDR: DynamicContext is faster for the use cases tested)

## Use cases
There are a variety of ways to retrieve the data using a DbContext - some more dynamic than others. This library address certain use cases not catered for by EF Core out the box.  

The library is `intended to be used in conjunction with, and complementing the EF Core DBContext functionality`. If The EF Core DbContext be only being used to leverage the functionality of this library, it might make more sense to use a completely different ORM to handle the data access.