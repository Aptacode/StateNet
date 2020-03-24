using System;
using Aptacode.StateNet.Engine;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Aptacode.StateNet.Network
{
    public class StatefulScriptCompiler<T>
    {
        private readonly Script<T> script;

        public StatefulScriptCompiler()
        {
            var options = ScriptOptions.Default;
            script = CSharpScript.Create<T>($"default ({typeof(T).FullName})", options, typeof(EngineHistory));
        }

        public Func<EngineHistory, T> Compile(string source)
        {
            var runner = script.ContinueWith<T>($"return {source};").CreateDelegate();
            return globals => runner(globals ?? new EngineHistory()).Result;
        }
    }
}