<p align="center">
  <img width="640" height="320" src="https://raw.githubusercontent.com/Timmoth/Aptacode.StateNet/dev/Resources/Images/StateNetBanner.jpg">
</p>

## A .Net Standard library used to model complicated State Machines

NuGet package

https://www.nuget.org/packages/Aptacode.StateNet/

Discord Group for development / help

https://discord.gg/D8MSXJB

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/bbdf96f5e1304d679e6addf01b2618a1)](https://www.codacy.com/manual/Timmoth/AptacodeStateNet?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Timmoth/AptacodeStateNet&amp;utm_campaign=Badge_Grade)

### Overview

The original goal of StateNet was to create a simple way to define and control the flow through pages of an application. Since its inception the library has grown versatile with many usecases.

### Usage

StateNet works by defining a network of states interlinked by the inputs that can be applied to them. 

#### How to Configure the Network
Determine all of the states you need and consider the relationship between them to determine the inputs for your system.
There are three approaches to configure the network:

#### 2) Network Creation Tool

<p align="center">
  <img width="1920" height="1080" src="https://raw.githubusercontent.com/Timmoth/Aptacode.StateNet/dev/Resources/Images/Demos/networkcreationtool.jpg">
</p>


#### 2) Object oriented
- Create a class which derives from 'Network'
- Define each State as a property on the class
- Use attributes on the State properties to define the relationships between them

```csharp
  public class CustomNetwork : Network
  {
	[StartState("Start")]
	[Connection("Left", "D1")]
	[Connection("Right", "D2")]
	public State StartTestState { get; set; }

	[StateName("D1")]
	[Connection("Next", "D1", "StateVisitCount(\"D2\") < 2 ? 1 : 0")]
	[Connection("Next", "End", "StateVisitCount(\"D2\") >= 2 ? 1 : 0")]
	public State Decision1TestState { get; set; }

	[StateName("D2")]
	[Connection("Next", "D1", "StateVisitCount(\"D2\") > 2 ? 1 : 0")]
	[Connection("Next", "D2", "StateVisitCount(\"D2\") <= 2 ? 1 : 0")]

	public State Decision2TestState { get; set; }

	[StateName("End")]
	public State EndTestState { get; set; }
  }
```

#### 3) Programmatic
```csharp
	IStateNetwork stateNetwork = new StateNetwork();
	var networkEditor = new StateNetworkEditor(stateNetwork);

	networkEditor.SetStart("ready");

	networkEditor.Always("ready", "Play", "playing");
	networkEditor.Always("ready", "Stop", "stopped");
	networkEditor.Always("playing", "Pause", "paused");
	networkEditor.Always("playing", "Stop", "stopped");
	networkEditor.Always("paused", "Play", "playing");
	networkEditor.Always("paused", "Stop", "stopped");

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
