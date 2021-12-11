using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace DynamicContext.Console
{
    public class DynamicContext<T> : DbContext where T : class
    {
        public DynamicContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\BlogDatabase.db");

            base.OnConfiguring(optionsBuilder);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var t = modelBuilder.Entity<T>().HasNoKey();

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
