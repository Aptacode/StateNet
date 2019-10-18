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

        /// <summary>
        ///     Get a list of all possible states
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetStates()
        {
            return _states;
        }

        /// <summary>
        ///     Convert an Enum into a StateCollection
        /// </summary>
        /// <typeparam name="TStates"></typeparam>
        /// <returns></returns>
        public static StateCollection FromEnum<TStates>() where TStates : Enum
        {
            return new StateCollection(Enum.GetNames(typeof(TStates)).ToList());
        }
    }
}