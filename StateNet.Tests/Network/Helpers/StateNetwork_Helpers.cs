using Aptacode.Expressions;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.PatternMatching;

namespace StateNet.Tests.Network.Helpers
{
    public static class StateNetwork_Helpers
    {
        public static readonly string StateB = "b";

        private static readonly ExpressionFactory<TransitionHistory> Expressions =
            new ExpressionFactory<TransitionHistory>();

        public static StateNetwork
            Minimal_Valid_Connected_StaticWeight_Network => //Make valid networks with the builder.
            NetworkBuilder.New.SetStartState("a").AddConnection("a", "1", "b", Expressions.Int(1))
                .AddConnection("b", "1", "a", Expressions.Int(1))
                .Build().Network;

        public static StateNetwork State_WithMultiple_Inputs_Network =>
            NetworkBuilder.New.SetStartState("a").AddConnection("a", "1", "b", Expressions.Int(1))
                .AddConnection("b", "2", "a", Expressions.Int(1))
                .Build().Network;

        public static StateNetwork Minimal_Valid_Connected_StaticWeight_Network_WithPattern =>
            new StateNetwork("a", StateNetworkDictionary_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkDictionary,
                new[] {new Pattern(StateB)});

        public static StateNetwork Invalid_StartState_Network =>
            new StateNetwork("c", StateNetworkDictionary_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkDictionary,
                new Pattern[0]); //Takes a state dictionary with states connected 'a' <-> 'b', sets start state to 'c'

        public static StateNetwork Invalid_ConnectionTargetState_Network =>
            new StateNetwork("a", StateNetworkDictionary_Helpers.Invalid_ConnectionTargetState_NetworkDictionary,
                new Pattern[0]);

        public static StateNetwork Invalid_ConnectionPatternState_Network =>
            new StateNetwork("a", StateNetworkDictionary_Helpers.Invalid_ConnectionPatternState_NetworkDictionary,
                new Pattern[0]);

        public static StateNetwork Invalid_ConnectionPatternInput_Network =>
            new StateNetwork("a", StateNetworkDictionary_Helpers.Invalid_ConnectionPatternInput_NetworkDictionary,
                new Pattern[0]);

        //To make this test even a thing I made StateNetwork.StartState settable, might not be the correct idea if the exception is thrown beforehand in the constructor anyway.
        public static StateNetwork Empty_Network =>
            new StateNetwork("", StateNetworkDictionary_Helpers.Empty_NetworkDictionary, new Pattern[0]);

        public static StateNetwork Invalid_Unreachable_State_Network =>
            new StateNetwork("a", StateNetworkDictionary_Helpers.Invalid_Unreachable_State_NetworkDictionary,
                new Pattern[0]);

        public static StateNetwork Invalid_UnusableInput_Network =>
            new StateNetwork("a", StateNetworkDictionary_Helpers.Invalid_UnusableInput_NetworkDictionary,
                new Pattern[0]);
    }
}