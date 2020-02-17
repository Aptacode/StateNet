using System;

namespace Aptacode.StateNet
{
    public class EnumNodeEngine<TStates, TActions> : NodeEngine
        where TStates : Enum
        where TActions : Enum
    {
        public EnumNodeEngine(EnumNodeGraph<TStates, TActions> nodeGraph) : base(nodeGraph)
        {
        }

        public void Apply(TActions action) => Apply(action.ToString());
    }
}