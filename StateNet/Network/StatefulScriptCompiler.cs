using System;
using Aptacode.StateNet.Engine;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Aptacode.StateNet.Network
{
    public class ScriptEvaluator
    {
        private readonly Script<int> script;

        public ScriptEvaluator()
        {
            var options = ScriptOptions.Default;
            script = CSharpScript.Create<int>($"default ({typeof(int).FullName})", options, typeof(EngineHistory));
        }

        public Func<EngineHistory, int> Compile(string source)
        {
            var runner = script.ContinueWith<int>($"return {source};").CreateDelegate();
            return globals => runner(globals ?? new EngineHistory()).Result;
        }
    }
}