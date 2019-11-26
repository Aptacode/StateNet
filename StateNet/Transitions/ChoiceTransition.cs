using Aptacode.StateNet.Exceptions;
using Aptacode.StateNet.Inputs;
using Aptacode.StateNet.States;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.Transitions
{
    public class Transition<TKey> : BaseTransition
        where TKey : IStructuralEquatable, IStructuralComparable, IComparable
    {
        private readonly Func<TKey, State> _choiceFunction;

        public Transition(State origin, Input input, TKey destinations, Func<TKey, State> choiceFunction) : this(origin,
                                                                                                                 input,
                                                                                                                 destinations,
                                                                                                                 choiceFunction,
                                                                                                                 string.Empty)
        { }

        public Transition(State origin,
                          Input input,
                          TKey destinations,
                          Func<TKey, State> choiceFunction,
                          string message) : base(origin, input, message)
        {
            Destinations = destinations;
            _choiceFunction = choiceFunction;
        }


        private List<State> DestinationsToList() => Destinations.GetType()
            .GetProperties()
            .Select(property => new State(property.GetValue(Destinations).ToString()))
            .ToList();

        public override State Apply()
        {
            var choice = _choiceFunction?.Invoke(Destinations);
            if(choice.HasValue)
            {
                return choice.Value;
            }

            throw new InvalidChoiceException();
        }

        public override string ToString() => $"Transition: {Origin}({Input})->{{ {string.Join("| ", DestinationsToList())} }}";

        public TKey Destinations { get; }
    }
}