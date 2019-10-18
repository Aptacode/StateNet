using Aptacode.StateNet.Transitions;

namespace Aptacode.StateNet.StateTransitionTable
{
    public interface IStateTransitionTable
    {
        /// <summary>
        /// Setup the TransitionTable for the given States and Inputs
        /// </summary>
        /// <param name="stateCollection"></param>
        /// <param name="inputCollection"></param>
        void Setup(StateCollection stateCollection, InputCollection inputCollection);

        /// <summary>
        /// Define a transition
        /// </summary>
        /// <param name="transition"></param>
        void Set(Transition transition);

        /// <summary>
        /// Get the transition for the given State and Input
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Transition Get(string state, string input);

        /// <summary>
        /// Remove the transition setting the transition at its State and Input to null
        /// </summary>
        /// <param name="transition"></param>
        void Clear(Transition transition);
    }
}