# Aptacode.StateNet

## A small .Net Standard library used to model simple State Machines

NuGet package

https://www.nuget.org/packages/Aptacode.StateNet/

Discord Group for development / help

https://discord.gg/D8MSXJB

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/bbdf96f5e1304d679e6addf01b2618a1)](https://www.codacy.com/manual/Timmoth/AptacodeStateNet?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Timmoth/AptacodeStateNet&amp;utm_campaign=Badge_Grade)

### Overview

The original goal of StateNet was to create a simple way to define and control the flow through pages of an application. Though since its inception the library has grown versatile with many potential usecases.

### Usage
#### 1) Configure the Network
- All possible states
- All possible actions that can be applied to each state
- All relations between states
- Set the start state

#### 2) Start the Engine
- Subscribe to relevant events: OnStarted, OnFinished, OnTransition or listen for specific State transitions
- Subscribe to StateEvents - OnTransition
- Subscribe to a specific State 
- Call Start()
- Call Apply(action) to move through the network

### Three approaches to configure a Network

#### 1) Object oriented
- Define a class which derives from Network
- Define each State as a property on the class
- Use attributes on the State properties to define the relationships between them

```csharp
  public class CustomNetwork : Network
  {
      [StateStart("Start")]
      [Connection("Left", "D1")]
      [Connection("Right", "D2")]
      public State StartTestState;

      [StateName("D1")]
      [Connection("Next", "D1", "Static:1")]
      [Connection("Next", "End", "Static:0")]
      public State Decision1TestState;

      [StateName("D2")]
      [Connection("Next", "D1", "VisitCount:D2,2,0,0,2")]
      [Connection("Next", "D2", "VisitCount:D2,2,1,1,0")]
      public State Decision2TestState;

      [StateName("End")]
      public State EndTestState;
  }
```
#### 2) Programmatic - string based
```csharp

  var network = new Network();
  network.StartState = network["Ready"];
  
  network["Ready", "Play"].Always(network["Playing"]);
  network["Playing", "Pause"].Always(network["Paused"]);
  network["Playing", "Stop"].Always(network["Stopped"]);
  network["Paused", "Play"].Always(network["Playing"]);
  network["Paused", "Stop"].Always(network["Stopped"]);

```
#### 3) Programmatic - strongly typed
```csharp

    public enum States { Ready, Playing, Paused, Stopped }
    public enum Actions { Play, Pause, Stop }
    
    var network = new EnumNetwork<States, Actions>();
    
    var Ready = network[States.Ready];
    var Playing = network[States.Playing];
    var Paused = network[States.Paused];
    var Stopped = network[States.Stopped];
    network.StartState = Ready;

    network[Ready, Actions.Play].Always(Playing);
    network[Playing, Actions.Pause].Always(Paused);
    network[Playing, Actions.Stop].Always(Stopped);
    network[Paused, Actions.Play].Always(Playing);
    network[Paused, Actions.Stop].Always(Stopped);
    
```

#### Using the Engine
```csharp

//Create and configure the Network using your prefered method
var network = ...

//Create the Engine
var engine = new Engine(network);

//Subscribe to the engines events
engine.OnFinished += (sender) => { ... }
engine.OnTransition += (sender) => { ... }
engine.Subscribe(network["Playing"], () => { ... });

//Start the Engine
engine.Start();

//Apply actions to move through the states
engine.Apply("Play");
...
engine.Apply("Stop");

```


## License

MIT License
