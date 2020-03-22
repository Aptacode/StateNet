using System;

namespace Aptacode.StateNet.Connections
{
    public class ConnectionWeight : IEquatable<ConnectionWeight>
    {
        private string _expression;

        private Func<EngineLog, int> _weightFunction;

        public ConnectionWeight() : this(1)
        {
        }

        public ConnectionWeight(int weight) : this(weight.ToString())
        {
        }

        public ConnectionWeight(string expression)
        {
            Expression = expression;
        }

        private static StatefulScriptCompiler<int> Compiler { get; } = new StatefulScriptCompiler<int>();

        public string Expression
        {
            get => _expression;
            set
            {
                _expression = string.IsNullOrEmpty(value) ? 1.ToString() : value;

                if (int.TryParse(_expression, out var weight))
                {
                    _weightFunction = _ => weight;
                }
                else
                {
                    try
                    {
                        _weightFunction = Compiler.Compile(_expression);
                    }
                    catch
                    {
                        _expression = "0";
                        _weightFunction = _ => 0;
                    }
                }
            }
        }

        public bool Equals(ConnectionWeight other)
        {
            return other != null && Expression.Equals(other.Expression);
        }

        public int GetWeight(EngineLog log)
        {
            var result = _weightFunction(log);
            return result >= 0 ? result : 0;
        }

        public override int GetHashCode()
        {
            return Expression.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is ConnectionWeight other && Equals(other);
        }

        public override string ToString()
        {
            return Expression;
        }
    }
}