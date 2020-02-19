<p align="center">
  <img width="600" height="200" src="https://raw.githubusercontent.com/Timmoth/Aptacode.StateNet/dev/Resources/Images/StateNetBanner.png">
</p>

## A .Net Standard library used to model complicated State Machines

NuGet package

https://www.nuget.org/packages/Aptacode.StateNet/

Discord Group for development / help

https://discord.gg/D8MSXJB

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/bbdf96f5e1304d679e6addf01b2618a1)](https://www.codacy.com/manual/Timmoth/AptacodeStateNet?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Timmoth/AptacodeStateNet&amp;utm_campaign=Badge_Grade)

### Overview

The original goal of StateNet was to create a simple way to define and control the flow through pages of an application. Since its inception the library has grown versatile with many potential usecases.

### Usage

StateNet works by defining a network of states interlinked by the actions which can be applied to them. 

#### How to Configure the Network
Determine all of the states you need and consider the relationship between them to determine the actions for your system.
There are two approaches to configure the network:

#### 1) Object oriented
- Create a class which derives from 'Network'
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

#### 2) Programmatic
Note* You can create a strongly typed network using enumerations OR a string based network.
```csharp

	//Strongly typed enumeration network
	//**********************************
	//Declare two enums consisting of each state and action
	public enum States { Ready, Playing, Paused, Stopped }
	public enum Actions { Play, Pause, Stop }

	//Create a new network
	var network1 = new EnumNetwork<States, Actions>();

	//Get each state
	var Ready = network1[States.Ready];
	var Playing = network1[States.Playing];
	var Paused = network1[States.Paused];
	var Stopped = network1[States.Stopped];
	
	//Set the start state
	network1.StartState = Ready;

	//Define the relationships between each state
	network1[Ready, Actions.Play].Always(Playing);
	network1[Playing, Actions.Pause].Always(Paused);
	network1[Playing, Actions.Stop].Always(Stopped);
	network1[Paused, Actions.Play].Always(Playing);
	network1[Paused, Actions.Stop].Always(Stopped);
	
	//String based network
	//**********************************
	var network2 = new Network();
	network2.StartState = network2["Ready"];

	network2["Ready", "Play"].Always(network2["Playing"]);
	network2["Playing", "Pause"].Always(network2["Paused"]);
	network2["Playing", "Stop"].Always(network2["Stopped"]);
	network2["Paused", "Play"].Always(network2["Playing"]);
	network2["Paused", "Stop"].Always(network2["Stopped"]);
```

#### Running the engine
Pass the configured network to the engine through its constructor.
Optionally subscribe to the engines events such as OnStarted, OnFinished, OnTransition or subscribe to specific State transitions.
Control the flow through the network by calling Start(), Stop() and Apply(action)

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
