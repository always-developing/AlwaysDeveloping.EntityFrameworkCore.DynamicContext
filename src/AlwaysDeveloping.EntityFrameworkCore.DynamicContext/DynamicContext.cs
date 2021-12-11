using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AlwaysDeveloping.EntityFrameworkCore.DynamicContext
{
    /// <summary>
    /// Wrapper class for the original db context which handles the managaement of the dynamic context
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DynamicContext<TContext> where TContext : DbContext
    {
        /// <summary>
        /// The original context
        /// </summary>
        private readonly DbContext _originalContext;

        /// <summary>
        /// The initilization options
        /// </summary>
        private readonly DynamicContextInitOptions<TContext> _runtimeInitAction;

        /// <summary>
        /// The service provider implementation
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initilizes a new instance of the DynamicContext class
        /// </summary>
        /// <param name="context">The original context</param>
        /// <param name="runtimeInitAction">The initlaization action for the context</param>
        /// <param name="serviceProvider">The service provider implementation</param>
        public DynamicContext(TContext context, DynamicContextInitOptions<TContext> runtimeInitAction, IServiceProvider serviceProvider)
        {
            _originalContext = context;
            _runtimeInitAction = runtimeInitAction;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Returns the original context
        /// </summary>
        public DbContext Context { get { return _originalContext; } }

        /// <summary>
        /// Creates the dyanmic context with a Dbset of type TEntity if it doesn't exist on the original context
        /// </summary>
        /// <typeparam name="TEntity">The type</typeparam>
        /// <returns>The DbSet of type TEntity</returns>
        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            // if the type is on the original context, then don't initialize the dyanmic context
            if (_originalContext.Model.FindEntityType(typeof(TEntity)) != null)
            {
                return _originalContext.Set<TEntity>();
            }

            var runtimeContext = new RuntimeContext<TEntity, TContext>(_runtimeInitAction, _serviceProvider);
            return runtimeContext.Set<TEntity>();
        }

        /// <summary>
        /// This is used for anonymous types. The example is used to infer the type
        /// </summary>
        /// <typeparam name="TEntity">An instance of the anonymous type to infer</typeparam>
        /// <param name="example">The type</param>
        /// <returns>The DbSet of type TEntity</returns>
        public DbSet<TEntity> Set<TEntity>(TEntity example) where TEntity : class
        {
            _ = example;

            // if the type is on the original context, then don't initialize the dyanmic context
            if (_originalContext.Model.FindEntityType(typeof(TEntity)) != null)
            {
                return _originalContext.Set<TEntity>();
            }

            var runtimeContext = new RuntimeContext<TEntity, TContext>(_runtimeInitAction, _serviceProvider);
            return runtimeContext.Set<TEntity>();
        }

        /// <summary>
        /// Creates the dyanmic context with a Dbset of type SimpleType with type TEntity
        /// </summary>
        /// <typeparam name="TEntity">The simple type</typeparam>
        /// <returns>The DbSet of type SimpleType</returns>
        public DbSet<SimpleType<TEntity>> ValueSet<TEntity>() where TEntity : struct 
        {
            if(!IsValidType(typeof(TEntity)))
            {
                throw new InvalidOperationException($"Type '{typeof(TEntity).Name}' is not supported");
            }

            var runtimeContext = GetInternalRuntimeContext(new SimpleType<TEntity>());
            return runtimeContext.Set<SimpleType<TEntity>>();
        }

        /// <summary>
        /// Creates the dyanmic context with a Dbset of type SimpleType with type string
        /// </summary>
        /// <typeparam name="TEntity">Of type string</typeparam>
        /// <returns>The DbSet of type SimpleType</returns>
        public DbSet<SimpleType<TEntity>> StringSet<TEntity>() where TEntity : IEnumerable<char>, IEnumerable, ICloneable, IComparable, IComparable<string>, IConvertible, IEquatable<string>
        {
            var runtimeContext = GetInternalRuntimeContext(new SimpleType<TEntity>());
            return runtimeContext.Set<SimpleType<TEntity>>();
        }

        /// <summary>
        /// Private method to get the internal dynamic context, infering the type from the example
        /// </summary>
        /// <typeparam name="TEntity">The type</typeparam>
        /// <param name="example">The example type used to infer TEntity</param>
        /// <returns>An instance of RuntimeContext</returns>
        private RuntimeContext<TEntity, TContext> GetInternalRuntimeContext<TEntity>(TEntity example) where TEntity : class
        {
            // just to get rid of message
            _ = example;
           return new RuntimeContext<TEntity, TContext>(_runtimeInitAction, _serviceProvider);
        }

        /// <summary>
        /// used to determine if the type is valid
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private static bool IsValidType(Type entityType)
        {
            // list of all valid types
            var validTypes = new List<Type>
            {
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(bool),
                typeof(char),
                typeof(Guid)
            };


            return validTypes.Contains(entityType);
        }
    }
}
