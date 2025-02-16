using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Diffex;
using Diffex.Abstractions;

using VerifyCS = Diffex.Analyzers.Test.CSharpCodeFixVerifier<
    Diffex.Analyzers.DiffexIgnorePrivateMemberAnalyzer,
    Diffex.Analyzers.DiffexAnalyzersCodeFixProvider>;

namespace Diffex.Analyzers.Test
{
    [TestClass]
    public class DiffexAnalyzersUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod2()
        {
            var test = @"
    using System;
    using System.Diagnostics;
    using Diffex;
    using Diffex.Abstractions;
    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
            [DiffexIgnore]
            private int IntProperty { get; set; }
        }
    }";

            var fixtest = @"
    using System;
    using System.Diagnostics;
    using Diffex;
    using Diffex.Abstractions;
    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
            private int IntProperty { get; set; }   
        }
    }";

            var expected = VerifyCS.Diagnostic("DIFFEX001");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
