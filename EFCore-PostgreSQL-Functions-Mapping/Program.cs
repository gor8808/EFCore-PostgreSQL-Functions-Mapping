using EFCore_PostgreSQL_Functions_Mapping.DataAccess;
using EFCore_PostgreSQL_Functions_Mapping.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore_PostgreSQL_Functions_Mapping
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var context = new AppDbContext();

            //await InitializeTestDataAsync(context);

            //apply filters
            var query = ApplyFilters(context.Entities.AsNoTracking());

            //apply orderings
            query = ApplyOrderings(query);

            var result = await query.ToListAsync();

            Console.WriteLine(result.Count);
        }

        private static IQueryable<Entity> ApplyFilters(IQueryable<Entity> entities)
        {
            return entities.Where(x =>
                Convert.ToString(CustomEFFunctions.JsonbPathQueryFirst(x.Properties, GetPropertySelectorJsonPath("Severity"))) == "\"High\"" &&
                Convert.ToInt32(CustomEFFunctions.JsonbPathQueryFirst(x.Properties, GetPropertySelectorJsonPath("Index"))) >= 1999000
                );
        }

        private static IQueryable<Entity> ApplyOrderings(IQueryable<Entity> entities)
        {
            return entities.OrderBy(x => CustomEFFunctions.JsonbPathQueryFirst(x.Properties, GetPropertySelectorJsonPath("Owner")))
                .ThenBy(x => CustomEFFunctions.JsonbPathQueryFirst(x.Properties, GetPropertySelectorJsonPath("Index")));
        }

        private static string GetPropertySelectorJsonPath(string propertyName)
        {
            return $"$[*] ? (@.Name == \"{propertyName}\").Value";
        }

        private static async Task InitializeTestDataAsync(AppDbContext context)
        {
            //Insure db is migrated
            await context.Database.MigrateAsync();

            var factory = new EntitiesDataFactory();

            //Create 2,000,000 test entities
            var testData = factory.CreateEntities(2000000);

            await context.Entities.AddRangeAsync(testData);

            await context.SaveChangesAsync();
        }
    }

    internal class Filter
    {
        public string PropertyName { get; private set; }
        public FilterOperator Operator { get; private set; }
        public object Value { get; private set; }

        public Filter(string propertyName, FilterOperator filterOperator, object value)
        {
            PropertyName = propertyName;
            Operator = filterOperator;
            Value = value;
        }
    }
    internal enum FilterOperator
    {
        Equal,
        NotEqual,
        Greater,
        Less
    }
}