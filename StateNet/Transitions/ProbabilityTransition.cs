//using Aptacode.StateNet.Exceptions;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Aptacode.StateNet.Inputs;
//using Aptacode.StateNet.States;
//namespace Aptacode.StateNet.Transitions
//{
//    public class ProbabilityTransition<TKey> : BaseTransition
//        where TKey : IStructuralEquatable, IStructuralComparable, IComparable
//    {
//        private readonly Func<TKey, State> _choiceFunction;

//        public ProbabilityTransition(State origin, Input input, TKey destinations, Func<TKey, State> choiceFunction) : this(origin,
//                                                                                                                    input,
//                                                                                                                    destinations,
//                                                                                                                    choiceFunction,
//                                                                                                                    string.Empty)
//        { }

//        public ProbabilityTransition(State origin,
//                          Input input,
//                          TKey destinations,
//                          Func<TKey, State> choiceFunction,
//                          string message) : base(origin, input, message)
//        {
//            Destinations = destinations;
//            _choiceFunction = choiceFunction;
//        }


//        private List<string> DestinationsToList() =>
//            // You can check if type of tuple is actually Tuple
// Destinations.GetType().GetProperties().Select(property => property.GetValue(Destinations).ToString()).ToList() ;

//        public override State Apply()
//        {
//            var choice = _choiceFunction?.Invoke(Destinations);

//            if(DestinationsToList().Contains(choice))
//            {
//                return choice;
//            }

//            throw new InvalidChoiceException();
//        }

//        public override string ToString() => $"Transition: {Origin}({Input})->{{ {string.Join("| ", DestinationsToList())} }}";

//        public TKey Destinations { get; }
//    }
//}