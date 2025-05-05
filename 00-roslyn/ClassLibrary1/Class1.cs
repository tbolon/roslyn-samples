using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
        public static void Test()
        {
            var x = "Hello World!";
            Console.WriteLine(x);
        }

        public string MyProperty { get; set; }

        public bool IsEmpty => !string.IsNullOrEmpty("");
    }
}
