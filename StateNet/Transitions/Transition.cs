using Aptacode.StateNet.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.Transitions
{
    public class Transition<TKey> : BaseTransition
        where TKey : IStructuralEquatable, IStructuralComparable, IComparable
    {
        private readonly Func<TKey, string> _choiceFunction;

        public Transition(string origin, string input, TKey destinations, Func<TKey, string> choiceFunction) : this(origin,
                                                                                                                    input,
                                                                                                                    destinations,
                                                                                                                    choiceFunction,
                                                                                                                    string.Empty)
        { }

        public Transition(string origin,
                          string input,
                          TKey destinations,
                          Func<TKey, string> choiceFunction,
                          string message) : base(origin, input, message)
        {
            Destinations = destinations;
            _choiceFunction = choiceFunction;
        }


        private List<string> DestinationsToList() =>
            // You can check if type of tuple is actually Tuple
 Destinations.GetType().GetProperties().Select(property => property.GetValue(Destinations).ToString()).ToList() ;

        public override string Apply()
        {
            var choice = _choiceFunction?.Invoke(Destinations);

            if(DestinationsToList().Contains(choice))
            {
                return choice;
            }

            throw new InvalidChoiceException();
        }

        public override string ToString() => $"Transition: {Origin}({Input})->{{ {string.Join("| ", DestinationsToList())} }}";

        public TKey Destinations { get; }
    }
}