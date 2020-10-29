﻿using System.Collections.Generic;
using Aptacode.Expressions.Integer;
using Aptacode.StateNet.Engine.Interpreter.Expressions;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;

namespace StateNet.Tests.Network.Helpers
{
    public static class StateNetworkDictionary_Helpers
    {
        public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            Valid_StaticWeight_NetworkDictionary =>
            new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
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
            };

        public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            Minimal_Valid_Connected_NetworkDictionary =>
            new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
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
            };

        public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            Invalid_ConnectionTargetState_NetworkDictionary =>
            new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
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
            };

        public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            Invalid_ConnectionPatternState_NetworkDictionary =>
            new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
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
            };

        public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            Invalid_ConnectionPatternInput_NetworkDictionary =>
            new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
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
            };

        public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            Invalid_NetworkDictionary_NoConnections =>
            new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>();        
            
        public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            Empty_NetworkDictionary =>
            new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>();

        public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            SingleState_NetworkDictionary =>
            new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>()
            {
                {
                    "a", new Dictionary<string, IReadOnlyList<Connection>>()
                }
            };
    }
}