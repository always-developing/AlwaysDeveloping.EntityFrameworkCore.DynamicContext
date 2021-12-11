using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace AlwaysDeveloping.EntityFrameworkCore.DynamicContext
{
    /// <summary>
    /// Internal dbcontext which is created dynamically
    /// </summary>
    /// <typeparam name="T">The type for which the DBSet will be created on the context</typeparam>
    /// <typeparam name="TContext">The context this context is modeled on</typeparam>
    internal class RuntimeContext<T, TContext> : DbContext where T : class where TContext : DbContext
    {
        /// <summary>
        /// The initialization
        /// </summary>
        private readonly DynamicContextInitOptions<TContext> _contextInitAction;

        /// <summary>
        /// DI servvice provider used for invoking initilizer
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="contextInitAction">The initialize options</param>
        /// <param name="serviceProvider">The service provider implementation</param>
        public RuntimeContext(DynamicContextInitOptions<TContext> contextInitAction, IServiceProvider serviceProvider)
        {
            _contextInitAction = contextInitAction;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// The method called when model is being configured
        /// </summary>
        /// <param name="optionsBuilder">The options builder</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // init the context based on how the initial TContext was initially initialized
            if (_contextInitAction.OptionsAction != null)
            {
                _contextInitAction.OptionsAction.Invoke(optionsBuilder);
            }
            else
            {
                _contextInitAction.OptionsActionDependencyInjection.Invoke(_serviceProvider, optionsBuilder);
            }

            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// The method called when model is creating
        /// </summary>
        /// <param name="modelBuilder">The model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var t = modelBuilder.Entity<T>().HasNoKey();

            //to support anonymous types, configure entity properties for read-only properties
            foreach (var prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!prop.CustomAttributes.Any(a => a.AttributeType == typeof(NotMappedAttribute)))
                {
                    t.Property(prop.Name);
                }

            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
