using System.Collections.Generic;

namespace Aptacode.StateNet.Engine {
    public class TransitionHistory
    {
        private readonly List<Transition> _transitions = new List<Transition>();

        public IReadOnlyList<Transition> Transitions => _transitions.AsReadOnly();

        public void Add(Transition transition)
        {
            _transitions.Add(transition);
        }
    }
}