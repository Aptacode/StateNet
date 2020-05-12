using System;
using System.Linq;
using System.Reflection;
using Aptacode.StateNet.Engine.History;
using Aptacode.StateNet.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace Aptacode.StateNet.Network.Connections
{
    public sealed class ConnectionWeightScriptCompiler
    {
        private static readonly Script<int> Script = GetScript();

        public static Script<int> GetScript()
        {
            var scriptOptions = ScriptOptions.Default;
            var mscorlib = typeof(object).GetTypeInfo().Assembly;
            var systemCore = typeof(Enumerable).GetTypeInfo().Assembly;

            var references = new[] {mscorlib, systemCore};
            scriptOptions = scriptOptions.AddReferences(references);

            using (var interactiveLoader = new InteractiveAssemblyLoader())
            {
                foreach (var reference in references)
                {
                    interactiveLoader.RegisterDependency(reference);
                }

                // Add namespaces
                scriptOptions = scriptOptions.AddImports("System");
                scriptOptions = scriptOptions.AddImports("System.Linq");
                scriptOptions = scriptOptions.AddImports("System.Collections.Generic");

                // Initialize script with custom interactive assembly loader
                return CSharpScript.Create<int>($"default ({typeof(int).FullName})", scriptOptions,
                    typeof(IEngineHistory), interactiveLoader);
            }
        }

        public static Func<IEngineHistory, int> Compile(string source)
        {
            var script = Script.ContinueWith<int>($"return {source};");

            var scriptRunner = script.CreateDelegate();

            return engineHistory =>
            {
                if (engineHistory == null)
                {
                    engineHistory = new EngineHistory();
                }

                return scriptRunner(engineHistory).Result;
            };
        }
    }
}