using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.TableMachine.Inputs;
using Aptacode.StateNet.TableMachine.States;

namespace Aptacode.StateNet.TableMachine.Transitions
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
            if (choice.HasValue)
            {
                return choice.Value;
            }

            throw new Exception();
        }

        public override string ToString() => $"Transition: {Origin}({Input})->{{ {string.Join("| ", DestinationsToList())} }}";

        public TKey Destinations { get; }
    }
}