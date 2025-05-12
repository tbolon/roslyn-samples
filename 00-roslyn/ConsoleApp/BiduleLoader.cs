namespace ConsoleApp
{
    internal class BiduleLoader
    {
        public MyPerson? LoadPerson(string id)
        {
            if(id == null)
                return null;

            if (id == string.Empty)
                return null;

            return new MyPerson { };
        }
    }
}
