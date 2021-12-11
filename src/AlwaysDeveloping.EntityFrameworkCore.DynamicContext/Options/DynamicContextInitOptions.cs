using Microsoft.EntityFrameworkCore;
using System;

namespace AlwaysDeveloping.EntityFrameworkCore.DynamicContext
{
    /// <summary>
    /// The options to store details on how the dynamic context should be initialized
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DynamicContextInitOptions<TContext> where TContext : DbContext
    {
        /// <summary>
        /// The action to initialize thge dynamic context
        /// </summary>
        public Action<DbContextOptionsBuilder> OptionsAction { get; set; } = null;

        /// <summary>
        /// The action to initialize thge dynamic context using the service provider implementation
        /// </summary>
        public Action<IServiceProvider, DbContextOptionsBuilder> OptionsActionDependencyInjection { get; set; } = null;
    }
}
