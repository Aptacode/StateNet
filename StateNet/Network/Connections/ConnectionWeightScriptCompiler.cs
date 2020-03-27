using System;
using Aptacode.StateNet.Engine.History;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Aptacode.StateNet.Network.Connections
{
    public sealed class ConnectionWeightScriptCompiler
    {
        private static readonly Script<int> _script = CSharpScript.Create<int>($"default ({typeof(int).FullName})",
            ScriptOptions.Default, typeof(EngineHistory));

        public static Func<EngineHistory, int> Compile(string source)
        {
            var script = _script.ContinueWith<int>($"return {source};");
            var scriptRunner = script.CreateDelegate();

            return engineHistory => scriptRunner(engineHistory ?? new EngineHistory()).Result;
        }
    }
}