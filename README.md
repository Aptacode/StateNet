# AptacodeStateNet

## A small .Net Standard library used to model simple Finite State Machines.

## Usage: 

### States and Actions

The state machine is configured using two generic type parameters: 'States' & 'Actions' 
- States defines all of the possible states the machine can be in.
- Actions defines all of the actions that can be applied to the state machine causeing a transition into a different state

The state machine takes its initial state as a constructor parameter.

Once you have created an instance of the state machine you need to define all possible transitions between states and
which actions cause each transition e.g the state 'Playing' will transition to 'Paused' when the 'Pause' action is applied.


```
//Define all the possible states and actions
public enum States { NotReady, Ready, Running, Paused };
public enum Actions { Setup, Start, Pause, Resume, Stop };

//Create an instance set to an initial state
StateMachine stateMachine = new StateMachine<States, Actions>(States.NotReady);

//Define all possible transitions
stateMachine.Define(
      new InvalidTransition<States, Actions>(
            States.NotReady, 
            Actions.Start, 
            "Must be Ready to Start"));

stateMachine.Define(
      new UnaryTransition<States, Actions>(
            States.NotReady, 
            Actions.Setup, 
            States.Ready,
            new Func<UnaryTransitionAcceptanceResult>(() => {
                  return new UnaryTransitionAcceptanceResult("Setup successful");
            }),
            "Setup"));

stateMachine.Define(
      new BinaryTransition<States, Actions>(
            States.Running, 
            Actions.Pause, 
            States.Paused, 
            States.NotReady, 
            new Func<BinaryTransitionAcceptanceResult>(() => {
                  if(canPause)
                        return new BinaryTransitionAcceptanceResult(BinaryChoice.Left, "Pause successful");
                  else
                        return new BinaryTransitionAcceptanceResult(BinaryChoice.Right, "Could not Pause");
            }),
            "Pause"));
            
//Subscribe to transitions
stateMachine.OnTransition += (s, e) => 
{ 
      status = string.Format("Old State: {0} Acton: {1} New State: {2}", e.OldState, e.Action, e.NewState);
};


//Apply actions to cause transitions
stateMachine.Apply(Actions.Setup);
stateMachine.Apply(Actions.Start);


```
## License
MIT License
