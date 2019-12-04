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

NodeMachine controls the flow through states by traversing a graph of nodes.
There are 5 types of Node: Unary, Binary, Ternary, Quaternary and End.
Each node has a GetNext function which returns the next node to visit. 
In a UnaryNode this function always returns the same DestinationNode.
An EndNode signifys a state with no possible transitions and so this function returns null. 
In all other Nodes this function can return more then one Node determined by a 'ChoiceFunction' the Choice function can return either a DeterministicChooser or a ProbabilisticChooser both of these classes returns a choice based on an enumeration of possible choices.
In the case of a DeterministicChooser the user defines the choice in a callback function.
In the case of a ProbabilisticChooser the user defines a distribution of weights for each possible choice which the node then uses to pick a (weighted) random choice.

To use this state machine you must first instantiate each node with its given name. 
Then for each node you define which nodes can be visited and a Chooser Function for how to decide which node IS visited when that node is exited. 
In order to move from one node to another you must subscribe to the 'OnVisited' event which is fired when a node is entered. From within the handler you define the unit of work to be executed when that node is executed, Once you are ready to visit the next node in the graph you must call the 'Exit' method of the current node.

To start the graph create an instance of 'NodeEngine' which you pass in through the constructor the start node, when your ready call the Start method to begin.

### Usage

```csharp

//Define all of the nodes each node can visit And the critera for deciding which to choose
var nodeGraph = new NodeGraph();
nodeGraph.SetStart("T1");
var T1 = nodeGraph.Add("T1", "U1", "U2", "B1", new TernaryProbabilityChooser(1, 1, 1));
var U1 = nodeGraph.Add("U1", "T1");
var U2 = nodeGraph.Add("U2", "T1");
var B1 = nodeGraph.Add("B1", "T1", "End1", new DeterministicChooser<BinaryChoice>(BinaryChoice.Item1));

T1.OnVisited += InstantTransition;
U1.OnVisited += InstantTransition;
U2.OnVisited += InstantTransition;
B1.OnVisited += InstantTransition;

_engine = new NodeEngine(nodeGraph);
_engine.Start();
_engine.OnFinished += (s) =>
{
...
};
  
private void InstantTransition(Node sender) => sender.Exit();

```

## License

MIT License
