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
        public IEnumerable<string> GetInputs()
        {
            return _inputs;
        }

        public static InputCollection FromEnum<TInputs>() where TInputs : Enum
        {
            return new InputCollection(Enum.GetNames(typeof(TInputs)).ToList());
        }
    }
}
