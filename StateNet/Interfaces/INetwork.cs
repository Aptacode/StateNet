using System.Collections.Generic;

namespace Aptacode.StateNet.Interfaces
{
    public interface INetwork
    {
        State StartState { get; set; }

        State this[string state] { get; }
        StateDistribution this[string state, string action] { get; }
        StateDistribution this[State state, string action] { get; }
        IEnumerable<State> GetAll();

        IEnumerable<State> GetEndStates();

        bool IsValid();

        IEnumerable<string> GetAllActions();
        IEnumerable<string> GetAllActions(State state);
    }
}