using CliWrap;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BuildInfo.Generator
{
    [Generator]
    public class BuildInfoGenerator : ISourceGenerator
    {
        private static readonly DiagnosticDescriptor MissingPropertyWarning = new DiagnosticDescriptor(
            id: "BUILDINFOGEN001",
            title: "Missing CompilerVisibleProperty",
            messageFormat: "Generator requires MSBuildProjectDirectory CompilerVisibleProperty to get Git SHA",
            category: "BuildInfoGenerator",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public void Execute(GeneratorExecutionContext context)
        {
            if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue($"build_property.MSBuildProjectDirectory", out var path))
            {
                context.ReportDiagnostic(Diagnostic.Create(MissingPropertyWarning, Location.None));
            }

            var buildTime = DateTime.Now;
            var gitSha = this.GetGitSha(path).Result;

            string source = $@"
using System;

namespace Build
{{
    public static class Info
    {{
        public static DateTime Timestamp {{ get; }} = new DateTime({buildTime.Ticks});
        public static string GitSha {{ get; }} = ""{gitSha}"";
    }}
}}
";
            context.AddSource("BuildInfo", source);
        }

        private async Task<string> GetGitSha(string path)
        {
            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();
            try
            {
                await Cli.Wrap("git")
                    .WithArguments("rev-parse --short HEAD")
                    .WithWorkingDirectory(path)
                    .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                    .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                    .ExecuteAsync();
                return stdOutBuffer.ToString().Trim();
            }
            catch
            {
                return "N/A";
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
