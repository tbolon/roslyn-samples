namespace MyApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var pierre = new MyPerson { Id = 2, Name = "Pierre" };
            var pierre2 = new MyPerson { Id = 2, Name = "Pierre" };

            Console.WriteLine(pierre == pierre2);
        }
    }
}