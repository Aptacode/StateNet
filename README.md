# Aptacode_StateMachine

## A small .Net Standard library used to model simple Finite State Machines.

## Usage: 


### Define two enums:
```
public enum States { NotReady, Ready, Running, Paused };
public enum Actions { Setup, Start, Pause, Resume, Stop };
```

### Instantiate the StateMachine given the initial state:
```
StateMachine stateMachine = new StateMachine<States, Actions>(States.NotReady);
```

### Define each transition
The 'TransitionAcceptanceResult' returned by the function defined in each transition determines the next state.

```
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
```

### Apply transitions
```
stateMachine.Apply(Actions.Setup);
stateMachine.Apply(Actions.Start);
```

## License
MIT License
