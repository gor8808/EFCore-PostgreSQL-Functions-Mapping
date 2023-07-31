using EFCore_PostgreSQL_Functions_Mapping.Entities.ValueObjects;

namespace EFCore_PostgreSQL_Functions_Mapping.Entities
{
    public class Entity
    {
        public Guid Id { get; private set; }
        public List<Property> Properties { get; private set; }

        public Entity(List<Property> properties)
        {
            Id = Guid.NewGuid();

            Properties = properties;
        }
    }
}
