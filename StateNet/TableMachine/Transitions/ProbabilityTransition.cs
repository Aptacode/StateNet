using Aptacode.StateNet.TableMachine.Inputs;
using Aptacode.StateNet.TableMachine.States;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.TableMachine.Transitions
{
    public class ProbabilityTransition<TKey> : BaseTransition
        where TKey : IStructuralEquatable, IStructuralComparable, IComparable
    {
        private readonly Dictionary<State, int> _stateWeightDistrobution;
        private readonly Action<TKey, Dictionary<State, int>> _updateProbabilitys;

        public ProbabilityTransition(State origin,
                                     Input input,
                                     TKey destinations,
                                     Action<TKey, Dictionary<State, int>> updateProbabilitys) : this(origin,
                                                                                                     input,
                                                                                                     destinations,
                                                                                                     updateProbabilitys,
                                                                                                     string.Empty)
        { }

        public ProbabilityTransition(State origin,
                                     Input input,
                                     TKey destinations,
                                     Action<TKey, Dictionary<State, int>> updateProbabilitys,
                                     string message) : base(origin, input, message)
        {
            Destinations = destinations;
            _updateProbabilitys = updateProbabilitys;
            _stateWeightDistrobution = new Dictionary<State, int>();
            foreach(var state in DestinationsToList())
            {
                _stateWeightDistrobution.Add(state, 1);
            }
        }


        private List<State> DestinationsToList() => Destinations.GetType()
            .GetProperties()
            .Select(property => new State(property.GetValue(Destinations).ToString()))
            .ToList();


        public override State Apply()
        {
            _updateProbabilitys?.Invoke(Destinations, _stateWeightDistrobution);

            var weightSum = _stateWeightDistrobution.Select(keyValue => keyValue.Value).Sum();
            var randomChoice = new Random(DateTime.Now.Millisecond).Next(1, weightSum - 1);

            var total = 0;
            foreach(var keyValue in _stateWeightDistrobution)
            {
                total += keyValue.Value;
                if(total >= randomChoice)
                {
                    return keyValue.Key;
                }
            }

            throw new Exception();
        }

        public override string ToString() => $"Transition: {Origin}({Input})->{{ {string.Join("| ", DestinationsToList())} }}";

        public TKey Destinations { get; }
    }
}