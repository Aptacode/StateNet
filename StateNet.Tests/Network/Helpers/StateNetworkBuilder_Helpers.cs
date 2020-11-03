using Aptacode.Expressions;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;

namespace StateNet.Tests.Network.Helpers
{
    public static class StateNetworkBuilder_Helpers
    {
        private static readonly ExpressionFactory<TransitionHistory> Expressions =
            new ExpressionFactory<TransitionHistory>();

        public static NetworkBuilder Minimal_Valid_Connected_StaticWeight_NetworkBuilder =>
            NetworkBuilder.New.SetStartState("a")
                .AddConnection("a", "1", "b", Expressions.Int(1));

        public static NetworkBuilder Empty_NetworkBuilder =>
            NetworkBuilder.New;

        public static NetworkBuilder SingleState_NetworkBuilder =>
            NetworkBuilder.New.SetStartState("a");
    }
}