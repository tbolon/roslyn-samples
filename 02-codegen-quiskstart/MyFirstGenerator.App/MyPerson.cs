namespace MyApp
{
    //[MyFirstGenerator.EqualsOperator]
    public partial class MyPerson
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public override bool Equals(object? obj)
        {
            if (obj is MyPerson other) return Id == other.Id && Name == other.Name;
            return base.Equals(obj);
        }

        public override int GetHashCode() => Id.GetHashCode() ^ Name.GetHashCode();
    }
}