using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.Core
{
    public class StateCollection
    {
        private readonly List<string> States;
        public StateCollection(IEnumerable<string> states)
        {
            States = new List<string>(states);
        }

        public IEnumerable<string> GetStates()
        {
            return States;
        }

        public static StateCollection FromEnum<States>() where States : Enum
        {
            return new StateCollection(Enum.GetNames(typeof(States)).ToList());
        }
    }
}
