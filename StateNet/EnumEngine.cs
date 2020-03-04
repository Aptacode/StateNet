using System;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet
{
    public class EnumEngine<TStates, TActions> : Engine
        where TStates : Enum
        where TActions : Enum
    {
        public EnumEngine(IRandomNumberGenerator randomNumberGenerator, EnumNetwork<TStates, TActions> network) : base(randomNumberGenerator, network)
        {
        }

        public void Apply(TActions action)
        {
            Apply(action.ToString());
        }
    }
}