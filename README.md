# AptacodeStateNet

## A small .Net Standard library used to model simple State Machines

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/bbdf96f5e1304d679e6addf01b2618a1)](https://www.codacy.com/manual/Timmoth/AptacodeStateNet?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Timmoth/AptacodeStateNet&amp;utm_campaign=Badge_Grade)

### Overview

There are two types of State Machine in State Net, both can be used to achieve the same result however they are both suited to different situations.

## TableMachine

In order to use the TableMachine you must define a StateCollection and InputCollection. There are two types of State/Input Collection: the first is indexed by a string key (the State or Input name) the other is indexed by an enum. 
The StateCollection contains all possible states the machine can be in at any given time e.g Playing, Paused, Stopped
The InputCollection contains all possible inputs which can be applied to the machine e.g Play, Pause, Stop

The TableMachine stores its transitions in a TransitionTable, implemented with a Dictionary or Dictionary's the outer dictionary uses a 'State' as a Key, the value associated with it being another dictionary indexed by an 'Input' key associated with a 'Transition' value.
There are two types of transition Table: 
DeterministicTransitionTable - which only allows UnaryTransitions i.e a transition from StateA to StateB on input1.
NonDeterministicTransitionTable - which allows Unary, Binary, Ternary, Quaternary... transitions i.e in the case of a TernaryTransition when Input2 is applied to State1 the result could be State2, State3, State4

All transitions for all states under each input are initialised to an 'InvalidTransition'. Each Valid transition must be defined through the TransitionTable.

Once a StateCollection, InputCollection & TransitionTable have all been defined a TableEngine can be created which allows the user to apply Inputs to the current state and determines which state the application should enter.

### Written Example

A state machine of a video playback application is in 'States.Playing'.

There is a BinaryTransition defined for 'States.Playing' with a trigger of 'Actions.Pause' and destinations: 'States.Paused' & 'States.Stopped'. 

The user defined a function which returns either BinaryChoice.Left or BinaryChoice.Right depending on if the application could pause the video.

when a user presses the pause button 'Actions.Pause' will be applied. 

Since the current state is 'States.Playing' the above transition will be applied, if the application could not pause, the funciton returns BinaryChoice.Right hence moving into 'States.Stopped'.

### Usage

```csharp
//Define all possible states and actions
public enum Inputs { Play, Pause, Stop }
public enum States { Begin, Playing, Paused, End }

//Define the State Collection
_stateCollection = new EnumStateCollection<States>();
//Define the Input Collection
_inputCollection = new EnumInputCollection<Inputs>();
//Define the Transition Table
_stateTransitionTable = new NonDeterministicTransitionTable(_stateCollection, _inputCollection);

//Define all possible transitions

//Invalid Transition
_stateTransitionTable.Set(_stateCollection[States.Begin], _inputCollection[Inputs.Pause], "Cannot Pause before the video is playing");

//Unary Transition
_stateTransitionTable.Set(_stateCollection[States.Begin],
                          _inputCollection[Inputs.Stop],
                          _stateCollection[States.End],
                          "Stop Video");

//Binary Transition
_stateTransitionTable.Set(_stateCollection[States.Begin],
                          _inputCollection[Inputs.Play],
                          _stateCollection[States.Playing],
                          _stateCollection[States.End],
                          (states) =>
                                    {
                                        if(_canPlay)
                                        {
                                            return states.Item1;
                                        }

                                        return states.Item2;
                                    }, "Play Video");

...

//Create an instance set to an initial state
_stateMachine = new TableEngine(_stateTransitionTable);

//Start the state machine giving it an initial state
_stateMachine.Start(_stateCollection[States.Begin]);

//When a transition is applied
stateMachine.OnTransition += (s, e) => 
{ 
      status = string.Format("Old State: {0} Acton: {1} New State: {2}", e.OldState, e.Action, e.NewState);
};


//Apply actions to cause a transition
_stateMachine.Apply(_inputCollection[Inputs.Play]);
_stateMachine.Apply(_inputCollection[Inputs.Play]);

```

## NodeMachine



## License

MIT License
