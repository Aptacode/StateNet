using System.Collections.Generic;
using Aptacode.Expressions;
using Aptacode.Expressions.Integer;
using Aptacode.StateNet.Engine.Interpreter.Expressions;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;

namespace StateNet.Tests.Network.Helpers
{
    
    public static class StateNetworkBuilder_Helpers
    {
        private static ExpressionFactory<TransitionHistory> _expressions = new ExpressionFactory<TransitionHistory>();

        public static NetworkBuilder Minimal_Valid_Connected_StaticWeight_NetworkBuilder =>
            NetworkBuilder.New.SetStartState("a")
            //.AddState("b")
            //.AddConnection("a", "1", "b", _expressions.Int(1))
            .AddConnection("a", "1", "b", _expressions.Int(1));

        public static NetworkBuilder Empty_NetworkBuilder =>
            NetworkBuilder.New;

        public static NetworkBuilder SingleState_NetworkBuilder =>
            NetworkBuilder.New.SetStartState("a");


        public static NetworkBuilder TwoState_Unconnected_NetworkBuilder =>
            NetworkBuilder.New.SetStartState("a")
            .AddState("b");
    }
}