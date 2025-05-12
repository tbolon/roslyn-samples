using System.Runtime.CompilerServices;

namespace MyFirstGenerator
{
    internal static class ModuleInitializer
    {
        // cf. https://github.com/VerifyTests/Verify.SourceGenerators?tab=readme-ov-file#initialize
        [ModuleInitializer]
        public static void Init() => VerifySourceGenerators.Initialize();
    }
}
