using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace VDT.Core.Tryable.Generators;

[Generator]
public class TestGenerator : IIncrementalGenerator {
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "GeneratedTryable.g.cs",
            SourceText.From(@"
namespace VDT.Core.Tryable;

public class GeneratedTryable { }
", Encoding.UTF8)));
    }
}
