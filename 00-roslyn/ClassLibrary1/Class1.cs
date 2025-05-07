using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
        // obsolete: test
        public static void Test() 
        {
            // OBSOLETE 14/05/2025
            // ce code doit être supprimé
            var x = "Hello World!";
            Console.WriteLine(x);
        }

        public string MyProperty { get; set; }

        public bool IsEmpty => !string.IsNullOrEmpty("");
    }
}
