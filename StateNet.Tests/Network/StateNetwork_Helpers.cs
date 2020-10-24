using System.Collections.Generic;
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
                                new Connection("b", 1)
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
                                new Connection("a", 1)
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
                                new Connection("b", 1)
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
                                new Connection("a", 1)
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
                                new Connection("c", 1)
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
                                new Connection("a", 1)
                            }
                        }
                    }
                }
            }, "a");

        public static StateNetwork Invalid_ConnectionPatternState_Network()
        {
            return new StateNetwork(new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            {
                {
                    "a", new Dictionary<string, IReadOnlyList<Connection>>
                    {
                        {
                            "1", new List<Connection>
                            {
                                new Connection("b", "s<c>", x => x)
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
                                new Connection("a", 1)
                            }
                        }
                    }
                }
            }, "a");
        }

        public static StateNetwork Invalid_ConnectionPatternInput_Network()
        {
            return new StateNetwork(new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            {
                {
                    "a", new Dictionary<string, IReadOnlyList<Connection>>
                    {
                        {
                            "1", new List<Connection>
                            {
                                new Connection("b", "s<b>i<3>", x => x)
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
                                new Connection("a", 1)
                            }
                        }
                    }
                }
            }, "a");
        }
    }
}