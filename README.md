# AptacodeStateNet

## A small .Net Standard library used to model simple State Machines

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/bbdf96f5e1304d679e6addf01b2618a1)](https://www.codacy.com/manual/Timmoth/AptacodeStateNet?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Timmoth/AptacodeStateNet&amp;utm_campaign=Badge_Grade)

### Overview

The original goal of StateNet was to create a simple way to define and control the flow through pages of an application. The library has many applications and since its inception I have used it in a number of different projects.

There are two types of State Machine in State Net, both can be used to achieve the same result. 

*NOTE NodeGraph has a much friendlier API and better performance. As such I am currently working on a wrapper around the NodeMachine to allow for the logic to be defined in a similar way to TableMachine and subsequently remove the TableMachine implementation alltogether.

## NodeMachine

NodeMachine controls the flow through states by traversing a graph of nodes.
The Network of nodes is defined by constructing a NodeGraph.
The connections between nodes are defined by two overloaded methods in NodeGraph.

ProbabilisticLink: Chooses the destination based on a collection of weighted probabilities.

DeterministicLink: Chooses the destination based on the users choice.

The first parameter of both methods is the name of the node for which the connection starts. All subsequent parameters are the names of nodes that can be reached from that connection. 


Both 'Link' methods return a respective derived class of NodeChooser<TChoice> where TChoice is a 'Choice' enumeration which contains an item for each node that can be reached by that connection. 

Choice Enumerations: 

Choices : Name

1       : UnaryChoice

2       : BinaryChoice

3       : TernaryChoice

4       : QuaternaryChoice

5       : QuinaryChoice

6       : SenaryChoice

7       : SeptenaryChoice

8       : OctaryChoice

9       : NonaryChoice


### Usage

```csharp

var nodeGraph = new NodeGraph();

//Define all the links between each node

//Deterministic Links - the node will always visit the selection of the chooser
nodeGraph.DeterministicLink("U1", "T1");
nodeGraph.DeterministicLink("U2", "T1");

//Probabilistic Links - the node will visit one of the choosers options depending on their weight
nodeGraph.ProbabilisticLink("B1", "T1", "End");

//Create a ProbabilisticLink. Then set custom weights for item 1 & 2 (the default weigt is 0)
var T1Chooser = nodeGraph.ProbabilisticLink("T1", "U1", "U2", "B1");
T1Chooser.SetWeight(TernaryChoice.Item1, 2);
T1Chooser.SetWeight(TernaryChoice.Item2, 0);

//Subscribe to OnVisited - make sure to call Node.Exit() at some point to move to the next Node in the graph
//The default behaviour is to instantly call Node.Exit()
nodeGraph.SubscribeOnVisited(initialNodeName, (sender) => { 
...
sender.Exit();
});

//Set the start node
nodeGraph.SetStart("T1");

//Create the NodeEngine from the graph
_engine = new NodeEngine(nodeGraph);

//Optionally listen for when the Engine reaches an End Node
_engine.OnFinished += (s) =>
{
...
};

_engine.Start();

```

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


## License

MIT License
