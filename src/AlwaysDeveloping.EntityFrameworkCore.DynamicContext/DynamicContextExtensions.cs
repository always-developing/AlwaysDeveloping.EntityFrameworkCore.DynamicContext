using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace AlwaysDeveloping.EntityFrameworkCore.DynamicContext
{
    /// <summary>
    /// Extension methods for the library
    /// </summary>
    public static class DynamicContextExtensions
    {

        /// <summary>
        /// Add the DB Context to the DI container, and configure a DynamicContext on top of the Context
        /// </summary>
        /// <typeparam name="TContext">The context</typeparam>
        /// <param name="serviceCollection">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="optionsAction">An optional action to configure the Microsoft.EntityFrameworkCore.DbContextOptions for the context</param>
        /// <param name="contextLifetime">The lifetime with which to register the DbContext service in the container</param>
        /// <param name="optionsLifetime">The lifetime with which to register the DbContextOptions service in the container</param>
        /// <returns>The same service collection so that multiple calls can be chained</returns>
        public static IServiceCollection AddDynamicContext<TContext>(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : DbContext
        {
            AddDynamicContent<TContext>(serviceCollection, optionsAction);
            return serviceCollection.AddDbContext<TContext, TContext>(optionsAction, contextLifetime, optionsLifetime);
        }

        /// <summary>
        /// Add the DB Context to the DI container, and configure a DynamicContext on top of the Context
        /// </summary>
        /// <typeparam name="TContextService">The class or interface that will be used to resolve the context from the container</typeparam>
        /// <typeparam name="TContextImplementation">The concrete implementation type to create</typeparam>
        /// <param name="serviceCollection">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="optionsAction">An optional action to configure the Microsoft.EntityFrameworkCore.DbContextOptions for the context</param>
        /// <param name="contextLifetime">The lifetime with which to register the DbContext service in the container</param>
        /// <param name="optionsLifetime">The lifetime with which to register the DbContextOptions service in the container</param>
        /// <returns>The same service collection so that multiple calls can be chained</returns>
        public static IServiceCollection AddDynamicContext<TContextService, TContextImplementation>(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContextImplementation : DbContext, TContextService
        {
            AddDynamicContent<TContextImplementation>(serviceCollection, optionsAction);
            return serviceCollection.AddDbContext<TContextService, TContextImplementation>(optionsAction, contextLifetime, optionsLifetime);
        }

        /// <summary>
        /// Add the DB Context to the DI container, and configure a DynamicContext on top of the Context
        /// </summary>
        /// <typeparam name="TContextService">The class or interface that will be used to resolve the context from the container</typeparam>
        /// <typeparam name="TContextImplementation">The concrete implementation type to create</typeparam>
        /// <param name="serviceCollection">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="contextLifetime">The lifetime with which to register the DbContext service in the container</param>
        /// <param name="optionsLifetime">The lifetime with which to register the DbContextOptions service in the container</param>
        /// <returns>The same service collection so that multiple calls can be chained</returns>
        public static IServiceCollection AddDynamicContext<TContextService, TContextImplementation>(this IServiceCollection serviceCollection, ServiceLifetime contextLifetime, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContextService : class where TContextImplementation : DbContext, TContextService
        {
            AddDynamicContent<TContextImplementation>(serviceCollection);
            return serviceCollection.AddDbContext<TContextService, TContextImplementation>((Action<IServiceProvider, DbContextOptionsBuilder>)null, contextLifetime, optionsLifetime);
        }

        /// <summary>
        /// Add the DB Context to the DI container, and configure a DynamicContext on top of the Context
        /// </summary>
        /// <typeparam name="TContext">The class or interface that will be used to resolve the context from the container</typeparam>
        /// <param name="serviceCollection">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="optionsAction">An optional action to configure the Microsoft.EntityFrameworkCore.DbContextOptions for the context</param>
        /// <param name="contextLifetime">The lifetime with which to register the DbContext service in the container</param>
        /// <param name="optionsLifetime">The lifetime with which to register the DbContextOptions service in the container</param>
        /// <returns>The same service collection so that multiple calls can be chained</returns>
        public static IServiceCollection AddDynamicContext<TContext>(this IServiceCollection serviceCollection, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : DbContext
        {
            AddDynamicContent<TContext>(serviceCollection, null, optionsAction);
            return serviceCollection.AddDbContext<TContext, TContext>(optionsAction, contextLifetime, optionsLifetime);
        }

        /// <summary>
        /// Add the DB Context to the DI container, and configure a DynamicContext on top of the Context
        /// </summary>
        /// <typeparam name="TContextService">The class or interface that will be used to resolve the context from the container</typeparam>
        /// <typeparam name="TContextImplementation">The concrete implementation type to create</typeparam>
        /// <param name="serviceCollection">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="optionsAction">An optional action to configure the Microsoft.EntityFrameworkCore.DbContextOptions for the context</param>
        /// <param name="contextLifetime">The lifetime with which to register the DbContext service in the container</param>
        /// <param name="optionsLifetime">The lifetime with which to register the DbContextOptions service in the container</param>
        /// <returns>The same service collection so that multiple calls can be chained</returns>
        public static IServiceCollection AddDynamicContext<TContextService, TContextImplementation>(this IServiceCollection serviceCollection, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContextImplementation : DbContext, TContextService
        {
            AddDynamicContent<TContextImplementation>(serviceCollection, null, optionsAction);
            return serviceCollection.AddDbContext<TContextService, TContextImplementation>(optionsAction, contextLifetime, optionsLifetime);
        }

        /// <summary>
        /// Convert the collection of SimpleType to a collection of type TEntity
        /// </summary>
        /// <typeparam name="TEntity">The simple type</typeparam>
        /// <param name="query">The query</param>
        /// <returns>The query of simple type</returns>
        public static IQueryable<TEntity> ToSimple<TEntity>(this IQueryable<SimpleType<TEntity>> query) where TEntity : struct, IComparable, IFormattable, IComparable<TEntity>, IEquatable<TEntity>
        {
            return query.Select(entity => entity.Value).AsQueryable();
        }

        /// <summary>
        /// Convert the collection of SimpleType to a collection of string
        /// </summary>
        /// <param name="query">The query</param>
        /// <returns>The query of simple type</returns>
        public static IQueryable<string> ToSimple(this IQueryable<SimpleType<string>> query) 
        {
            return query.Select(entity => entity.Value).AsQueryable();
        }

        /// <summary>
        /// Private method to handle adding the DynamicContext and options
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="optionsAction"></param>
        /// <param name="optionsActionDependencyInjection"></param>
        private static void AddDynamicContent<TContext>(IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction = null, Action<IServiceProvider, DbContextOptionsBuilder> optionsActionDependencyInjection = null) where TContext : DbContext
        {
            serviceCollection.AddScoped<DynamicContext<TContext>>();

            // If no action, then it would need to be added manually
            if(optionsAction == null && optionsActionDependencyInjection == null)
            {
                return;
            }

            var options = new DynamicContextInitOptions<TContext>
            {
                OptionsAction = optionsAction,
                OptionsActionDependencyInjection = optionsActionDependencyInjection
            };

            serviceCollection.AddSingleton(options);
        }
    }
}
