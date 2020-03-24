using System;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.Engine
{
    public class EnumStateNetEngine<TState, TInput> : StateNetEngine
        where TState : Enum
        where TInput : Enum
    {
        public EnumStateNetEngine(IRandomNumberGenerator randomNumberGenerator, IStateNetwork stateNetwork) : base(
            randomNumberGenerator,
            stateNetwork)
        {
        }

        public void Apply(TInput input)
        {
            Apply(input.ToString());
        }
    }
}