﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.TableMachine.Inputs
{
    public class InputCollection : HashSet<Input>
    {
        public InputCollection(IEnumerable<Input> inputs) : base(inputs) { }

        public Input this[string key] => this.FirstOrDefault(input => input.Name.Equals(key));
    }

    public class EnumInputCollection<TInputs> : InputCollection
        where TInputs : Enum
    {
        public EnumInputCollection() : base(Enum.GetNames(typeof(TInputs)).Select(name => new Input(name)).ToList()) { }

        public Input this[TInputs key] => this.FirstOrDefault(input => input.Name.Equals(key.ToString()));
    }
}