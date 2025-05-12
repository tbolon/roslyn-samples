namespace ConsoleApp
{
    public class MyPerson
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public override bool Equals(object? obj)
        {
            // 👇 implémentation standard de Equals
            if (obj is MyPerson other) return Id == other.Id && Name == other.Name;
            return base.Equals(obj);
        }

        public override int GetHashCode() => Id.GetHashCode() ^ Name.GetHashCode();

        // 👇 permet d'utiliser == pour utiliser person1.Equals(person2)
        // par défaut c'est une égalité par référence (pointeur) qui est utilisée
        public static bool operator ==(MyPerson left, MyPerson right) => left?.Equals(right) == true;

        public static bool operator !=(MyPerson left, MyPerson right) => !(left == right);
    }
}
