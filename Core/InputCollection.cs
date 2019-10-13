using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aptacode.StateNet.Core
{
    public class InputCollection
    {
        private readonly List<string> Inputs;

        public InputCollection(IEnumerable<string> inputs)
        {
            Inputs = new List<string>(inputs);
        }
        public IEnumerable<string> GetInputs()
        {
            return Inputs;
        }

        public static InputCollection FromEnum<Inputs>() where Inputs : Enum
        {
            return new InputCollection(Enum.GetNames(typeof(Inputs)).ToList());
        }
    }
}
