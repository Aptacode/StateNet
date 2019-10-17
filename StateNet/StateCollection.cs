using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet
{
    public class StateCollection
    {
        private readonly List<string> _states;

        public StateCollection(IEnumerable<string> states)
        {
            _states = new List<string>(states);
        }

        public IEnumerable<string> GetStates()
        {
            return _states;
        }

        public static StateCollection FromEnum<TStates>() where TStates : Enum
        {
            return new StateCollection(Enum.GetNames(typeof(TStates)).ToList());
        }
    }
}