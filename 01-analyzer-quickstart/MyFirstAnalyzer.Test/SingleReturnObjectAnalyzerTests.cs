using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = MyFirstAnalyzer.Helpers.CSharpAnalyzerVerifier<MyFirstAnalyzer.SingleReturnObjectAnalyzer>;

namespace MyFirstAnalyzer.Test
{
    [TestClass]
    public class SingleReturnObjectAnalyzerTests
    {
        [TestMethod]
        public async Task Baseline()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        class MyClassLoader
        {
            public void LoadMy()
            {
            }
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task Single_Return_Null()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        class MyClassLoader
        {
            public string LoadMy()
            {
                return null;
            }
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task Single_Return_NotNull()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        class MyClassLoader
        {
            public string LoadMy()
            {
                return ""test"";
            }
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task Multiple_Return_NotNull()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        class MyClassLoader
        {
            public string LoadMy(string id)
            {
                if (string.IsNullOrEmpty(id))
                    return null;

                if (id == ""test"")
                {
                    {|#0:return|} ""TEST"";
                }

                {|#1:return|} id;
            }
        }
    }";

            var expected1 = VerifyCS.Diagnostic(SingleReturnObjectAnalyzer.DiagnosticId).WithLocation(0);
            var expected2 = VerifyCS.Diagnostic(SingleReturnObjectAnalyzer.DiagnosticId).WithLocation(1);
            await VerifyCS.VerifyAnalyzerAsync(test, expected1, expected2);
        }
    }
}
