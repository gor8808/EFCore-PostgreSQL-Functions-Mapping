namespace EFCore_PostgreSQL_Functions_Mapping.Entities.ValueObjects
{
    public class Property
    {
        public string Name { get; private set; }
        public object Value { get; private set; }

        public Property(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
