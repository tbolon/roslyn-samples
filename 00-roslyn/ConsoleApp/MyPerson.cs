namespace ConsoleApp
{
    public class MyPerson
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public override bool Equals(object? obj)
        {
            if (obj is MyPerson other) return Id == other.Id && Name == other.Name;
            return base.Equals(obj);
        }

        public override int GetHashCode() => Id.GetHashCode() ^ Name.GetHashCode();

        public static bool operator ==(MyPerson left, MyPerson right) => left.Equals(right);

        public static bool operator !=(MyPerson left, MyPerson right) => left.Equals(right);
    }
}
