namespace ConsoleApp
{
    class BIDULELOADER
    {
        public string? LoadMy(string id)
        {
            if (id == null)
                return null;

            if (id == string.Empty)
                return null;

            if (id == "test")
                return "TEST";

            return id;
        }
    }
}
