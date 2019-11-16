using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet
{
    public class InputCollection : HashSet<string>
    {
        public InputCollection(IEnumerable<string> inputs) : base(inputs) { }

        /// <summary>
        /// Converts an Enum into a InputCollection
        /// </summary>
        /// <typeparam name="TInputs"></typeparam>
        /// <returns></returns>
        public static InputCollection FromEnum<TInputs>()
            where TInputs : Enum => new InputCollection(Enum.GetNames(typeof(TInputs)).ToList()) ;
    }
}