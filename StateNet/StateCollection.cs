using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet
{
    public class StateCollection : HashSet<string>
    {
        public StateCollection(IEnumerable<string> states) : base(states) { }

        /// <summary>
        /// Convert an Enum into a StateCollection
        /// </summary>
        /// <typeparam name="TStates"></typeparam>
        /// <returns></returns>
        public static StateCollection FromEnum<TStates>()
            where TStates : Enum => new StateCollection(Enum.GetNames(typeof(TStates)).ToList()) ;
    }
}