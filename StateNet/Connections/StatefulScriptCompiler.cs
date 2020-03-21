using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Aptacode.StateNet.Connections
{
    public class StatefulScriptCompiler<T>
    {
        private Script<T> script;

        public StatefulScriptCompiler()
        {
            var options = ScriptOptions.Default;
            script = CSharpScript.Create<T>($"default ({typeof(T).FullName})", options, typeof(EngineLog));
        }

        public Func<EngineLog, T> Compile(string source)
        {
            script = script.ContinueWith<T>($"return {source};");
            var runner = script.CreateDelegate();
            return globals =>
            {
                if (globals == null)
                {
                    globals = new EngineLog();
                }

                return runner(globals).Result;
            };
        }
    }
}