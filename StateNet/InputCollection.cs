using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet
{
    public class InputCollection
    {
        private readonly List<string> _inputs;

        public InputCollection(IEnumerable<string> inputs)
        {
            _inputs = new List<string>(inputs);
        }

        /// <summary>
        ///     Returns a the list of available inputs
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetInputs()
        {
            return _inputs;
        }

        /// <summary>
        ///     Converts an Enum into a InputCollection
        /// </summary>
        /// <typeparam name="TInputs"></typeparam>
        /// <returns></returns>
        public static InputCollection FromEnum<TInputs>() where TInputs : Enum
        {
            return new InputCollection(Enum.GetNames(typeof(TInputs)).ToList());
        }
    }
}