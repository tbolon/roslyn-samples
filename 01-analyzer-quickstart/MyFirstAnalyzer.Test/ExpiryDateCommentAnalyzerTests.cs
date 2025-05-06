using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = MyFirstAnalyzer.Helpers.CSharpAnalyzerVerifier<MyFirstAnalyzer.ExpiryDateCommentAnalyzer>;

namespace MyFirstAnalyzer.Test
{
    [TestClass]
    public class ExpiryDateCommentAnalyzerTests
    {
        /// <summary>
        /// No diagnostics expected to show up.
        /// </summary>
        [TestMethod]
        public async Task Baseline()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        class MyClass
        {
            public void MyMethod()
            {
            }
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task WithComment_Expired()
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
        class MyClass
        {
            // obsolete: 31/12/2024
            public void {|#0:MyMethod|}()
            {
            }
        }
    }";
            var expected = VerifyCS.Diagnostic(ExpiryDateCommentAnalyzer.DiagnosticId).WithLocation(0).WithArguments("31/12/2024");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task WithComment_Valid()
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
        class MyClass
        {
            // obsolete: 31/12/2025
            public void MyMethod()
            {
            }
        }
    }";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}
