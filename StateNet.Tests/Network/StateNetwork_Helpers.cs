using System.Collections.Generic;
using Aptacode.Expressions.Integer;
using Aptacode.StateNet.Engine.Interpreter.Expressions;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;

namespace StateNet.Tests.Network
{
    public static class StateNetwork_Helpers
    {
        public static StateNetwork Valid_StaticWeight_Network() =>
            new StateNetwork(new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            {
                {
                    "a", new Dictionary<string, IReadOnlyList<Connection>>
                    {
                        {
                            "1", new List<Connection>
                            {
                                new Connection("b", new ConstantInteger<TransitionHistory>(1))
                            }
                        }
                    }
                },
                {
                    "b", new Dictionary<string, IReadOnlyList<Connection>>
                    {
                        {
                            "1", new List<Connection>
                            {
                                new Connection("a", new ConstantInteger<TransitionHistory>(1))
                            }
                        }
                    }
                }
            }, "a");

        public static StateNetwork Invalid_Detached_StartState_Network() =>
            new StateNetwork(new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            {
                {
                    "a", new Dictionary<string, IReadOnlyList<Connection>>
                    {
                        {
                            "1", new List<Connection>
                            {
                                new Connection("b", new ConstantInteger<TransitionHistory>(1))
                            }
                        }
                    }
                },
                {
                    "b", new Dictionary<string, IReadOnlyList<Connection>>
                    {
                        {
                            "1", new List<Connection>
                            {
                                new Connection("a", new ConstantInteger<TransitionHistory>(1))
                            }
                        }
                    }
                }
            }, "c");

        public static StateNetwork Invalid_ConnectionTargetState_Network() =>
            new StateNetwork(new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            {
                {
                    "a", new Dictionary<string, IReadOnlyList<Connection>>
                    {
                        {
                            "1", new List<Connection>
                            {
                                new Connection("c", new ConstantInteger<TransitionHistory>(1))
                            }
                        }
                    }
                },
                {
                    "b", new Dictionary<string, IReadOnlyList<Connection>>
                    {
                        {
                            "1", new List<Connection>
                            {
                                new Connection("a", new ConstantInteger<TransitionHistory>(1))
                            }
                        }
                    }
                }
            }, "a");

        public static StateNetwork Invalid_ConnectionPatternState_Network() =>
            new StateNetwork(new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            {
                {
                    "a", new Dictionary<string, IReadOnlyList<Connection>>
                    {
                        {
                            "1", new List<Connection>
                            {
                                new Connection("b", new TransitionHistoryMatchCount("c"))
                            }
                        }
                    }
                },
                {
                    "b", new Dictionary<string, IReadOnlyList<Connection>>
                    {
                        {
                            "1", new List<Connection>
                            {
                                new Connection("a", new ConstantInteger<TransitionHistory>(1))
                            }
                        }
                    }
                }
            }, "a");

        public static StateNetwork Invalid_ConnectionPatternInput_Network() =>
            new StateNetwork(new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            {
                {
                    "a", new Dictionary<string, IReadOnlyList<Connection>>
                    {
                        {
                            "1", new List<Connection>
                            {
                                new Connection("b", new TransitionHistoryMatchCount("bi"))
                            }
                        }
                    }
                },
                {
                    "b", new Dictionary<string, IReadOnlyList<Connection>>
                    {
                        {
                            "1", new List<Connection>
                            {
                                new Connection("a", new ConstantInteger<TransitionHistory>(1))
                            }
                        }
                    }
                }
            }, "a");
    }
}