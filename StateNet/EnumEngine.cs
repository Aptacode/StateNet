using System;

namespace Aptacode.StateNet
{
    public class EnumEngine<TStates, TActions> : Engine
        where TStates : Enum
        where TActions : Enum
    {
        public EnumEngine(EnumNetwork<TStates, TActions> network) : base(network)
        {
        }

        public void Apply(TActions action) => Apply(action.ToString());
    }
}