using EFCore_PostgreSQL_Functions_Mapping.Entities;
using EFCore_PostgreSQL_Functions_Mapping.Entities.ValueObjects;

namespace EFCore_PostgreSQL_Functions_Mapping
{
    public class EntitiesDataFactory
    {
        private readonly Random _rnd = new Random();
        private readonly Dictionary<string, object[]> _properties = new()
        {
            {"Severity", new[] {"High", "Medium", "Low"} },
            {"Owner", new[] { "Saige", "Bowen", "Leighton", "Franklin", "Marceline" } },
        };

        public IEnumerable<Entity> CreateEntities(int count)
        {
            for (int i = 0; i < count; i++)
            {
                //Create random properties
                var properties = new List<Property>()
                {
                    new("Severity", PickRandom(_properties["Severity"])),
                    new("Owner", PickRandom(_properties["Owner"])),
                    new("Index", i),
                    //Add one more minute to each CreatedDate to have some unique data
                    new("CreatedDate", DateTime.UtcNow.AddMinutes(i))
                };

                yield return new Entity(properties);
            }
        }

        private TObject PickRandom<TObject>(IList<TObject> obj)
            => obj[_rnd.Next(0, obj.Count)];
    }
}
