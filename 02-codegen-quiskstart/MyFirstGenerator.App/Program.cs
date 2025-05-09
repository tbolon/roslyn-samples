namespace MyApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var pierre = new MyPerson { Id = 2, Name = "Pierre" };
            var pierre2 = new MyPerson { Id = 2, Name = "Pierre" };

            Console.WriteLine($"Pierre 1 == Pierre 2 ? {pierre == pierre2}");
        }
    }
}