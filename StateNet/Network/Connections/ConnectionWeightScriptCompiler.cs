using System;
using Aptacode.StateNet.Engine.History;
using Aptacode.StateNet.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Aptacode.StateNet.Network.Connections
{
    public sealed class ConnectionWeightScriptCompiler
    {
        private static readonly Script<int> _script = CSharpScript.Create<int>($"default ({typeof(int).FullName})",
            ScriptOptions.Default, typeof(IEngineHistory));

        public static Func<IEngineHistory, int> Compile(string source)
        {
            var script = _script.ContinueWith<int>($"return {source};");
            var scriptRunner = script.CreateDelegate();

            return engineHistory =>
            {
                if (engineHistory == null)
                    engineHistory = new EngineHistory();

                return scriptRunner(engineHistory).Result;
            };
        }
    }
}