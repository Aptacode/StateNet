# AptacodeStateNet

## A small .Net Standard library used to model simple Finite State Machines

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/bbdf96f5e1304d679e6addf01b2618a1)](https://www.codacy.com/manual/Timmoth/AptacodeStateNet?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Timmoth/AptacodeStateNet&amp;utm_campaign=Badge_Grade)

### Overview

The state machine is configured using a StateTransitionTable which in turn takes a set of 'States' & 'Inputs' which can be either user defined enums, or string lists.


-  States: Each possible state that the machine can be in at a given time.

-  Inputs: All of the actions that can an be applied to the state machine to induce a transitions between states.

The state machine takes its initial state as a constructor parameter.

Once an instance of StateTransitionTable is initialised, all possible transitions between states must be defined (Note* all transitions  default to an 'InvalidTransition')
A transition is triggered based on which state the machine is currently in and the input which was applied.
Depending on the type of transition the user can define their own logic to determine which of the transition's possible destination states is entered. For example: the BinaryTransition takes a function callback which returns either BinaryChoice.Left or BinaryChoice.Right to determine which state to enter.

There are four types of transition:

-  InvalidTransition an input which cannot be applied to a state.
-  UnaryTransition when an input being applied to a state always results in a transition to exactly one state.
-  BinaryTransition when an input being applied to a state can cause the state machine to move to one of two states states depending 
on some user defined function callback.
-  NaryTransition when an input causes a transition to one of many possible states chosen by a user defined function callback.

### Written Example

A state machine of a video playback application is in 'States.Playing'.

There is a BinaryTransition defined for 'States.Playing' with a trigger of 'Actions.Pause' and destinations: 'States.Paused' & 'States.Stopped'. 

The user defined a function which returns either BinaryChoice.Left or BinaryChoice.Right depending on if the application could pause the video.

when a user presses the pause button 'Actions.Pause' will be applied. 

Since the current state is 'States.Playing' the above transition will be applied, if the application could not pause, the funciton returns BinaryChoice.Right hence moving into 'States.Stopped'.

### Usage

```csharp
//Define all possible states and actions
public enum States { NotReady, Ready, Running, Paused };
public enum Actions { Setup, Start, Pause, Resume, Stop };
var transitionTable = new EnumStateTransitionTable<States, Inputs>();

//Create an instance set to an initial state
var stateMachine = new StateMachine(transitionTable, States.NotReady);

//Define all possible transitions

//Invalid Transition
transitionTable.Define(
            States.NotReady, 
            Actions.Start, 
            "Must be Ready to Start"));
            
//Unary Transition
transitionTable.Define(
            States.NotReady, 
            Actions.Setup, 
            States.Ready,
            "Setup"));

//Binary Transition
transitionTable.Define(
            States.Running, 
            Actions.Pause, 
            States.Paused, 
            States.NotReady,
            () =>
            {
                if(_canPlay)
                {
                    return BinaryChoice.Left;
                }

                return BinaryChoice.Right;
            });
            "Try Pause"));

//When a transition is applied
stateMachine.OnTransition += (s, e) => 
{ 
      status = string.Format("Old State: {0} Acton: {1} New State: {2}", e.OldState, e.Action, e.NewState);
};


//Apply actions to cause a transition
stateMachine.Apply(Actions.Setup);
stateMachine.Apply(Actions.Start);


```

## License

MIT License
