using EFCore_PostgreSQL_Functions_Mapping.Entities;
using EFCore_PostgreSQL_Functions_Mapping.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace EFCore_PostgreSQL_Functions_Mapping.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Entity> Entities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=EntitiesDataDB;Username=postgres;Password=psw");

            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<Entity>();

            entityBuilder.HasKey(e => e.Id);

            entityBuilder.Property(x => x.Properties)
               .HasColumnType("jsonb")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => JsonConvert.DeserializeObject<List<Property>>(v));


            //Map JsonbPathQueryFirst to psql jsonb_path_query_first function
            modelBuilder.HasDbFunction(typeof(CustomEFFunctions).GetMethod(nameof(CustomEFFunctions.JsonbPathQueryFirst)), builder =>
            {
                builder.HasParameter("json").HasStoreType("jsonb");

                builder.HasTranslation(args =>
                {
                    return new SqlFunctionExpression("jsonb_path_query_first", args, false, args.Select(x => false), typeof(string), null);
                });
            });

            base.OnModelCreating(modelBuilder);
        }

        
    }
}
