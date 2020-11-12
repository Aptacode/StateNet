using Aptacode.Expressions.Bool.Comparison;
using Aptacode.Expressions.Integer;
using Aptacode.StateNet.Engine.Transitions;
using System.Collections.Generic;

namespace Aptacode.StateNet.Network
{
    public class Connection
    {
        public Connection(string target, IIntegerExpression<TransitionHistory> expression)
        {
            Target = target;
            Expression = expression;
        }

        public IIntegerExpression<TransitionHistory> Expression { get; }

        public string Target { get; }

        //public class ConnectionEqualityComparer : IEqualityComparer<Connection> Something to ponder, might not really be necessary.
        //{
        //    public bool Equals (Connection c1, Connection c2)
        //    {
        //        if (c1 == null && c2 == null)
        //            return true;
        //        else if (c1 == null || c2 == null)
        //            return false;
        //        else if (c1.Target == c2.Target && c1.Expression.Interpret(TransitionHistory)

        //    }
        //}
    }
}