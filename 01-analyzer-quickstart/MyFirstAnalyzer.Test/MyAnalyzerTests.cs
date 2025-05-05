using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = MyFirstAnalyzer.Helpers.CSharpCodeFixVerifier<MyFirstAnalyzer.MyAnalyzer, MyFirstAnalyzer.MyAnalyzerCodeFixProvider>;

namespace MyFirstAnalyzer.Helpers
{
    [TestClass]
    public class MyAnalyzerTests
    {
        /// <summary>
        /// No diagnostics expected to show up.
        /// </summary>
        [TestMethod]
        public async Task Baseline()
        {
            var test = "";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        /// <summary>
        /// Diagnostic and CodeFix both triggered and checked for
        /// </summary>
        [TestMethod]
        public async Task Detect_And_Fix()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:TypeName|}
        {   
        }
    }";

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";

            var expected = VerifyCS.Diagnostic(MyAnalyzer.DiagnosticId).WithLocation(0).WithArguments("TypeName");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
