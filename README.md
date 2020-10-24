<p align="center">
  <img width="640" height="320" src="https://raw.githubusercontent.com/Timmoth/Aptacode.StateNet/Development/Resources/Images/StateNetBanner.jpg">
</p>

## A .Net Standard library used to model complicated State Machines

[![Discord Server](https://img.shields.io/discord/533275703882547200?logo=discord)](https://discord.gg/D8MSXJB)
[![NuGet](https://img.shields.io/nuget/v/Aptacode.StateNet.svg?style=flat)](https://www.nuget.org/packages/Aptacode.StateNet/)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/1d08da0b20b0407682ed6cf71f7bd3a1)](https://www.codacy.com/gh/Aptacode/StateNet/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Aptacode/StateNet&amp;utm_campaign=Badge_Grade)
![last commit](https://img.shields.io/github/last-commit/Timmoth/Aptacode.StateNet?style=flat-square&cacheSeconds=86000)
[![Build Status](https://dev.azure.com/Aptacode/StateNet/_apis/build/status/Aptacode.StateNet?branchName=Development)](https://dev.azure.com/Aptacode/StateNet/_build/latest?definitionId=21&branchName=Development)

### Overview

StateNet's primary purpose is to create a simple way to define and control the flow through pages of an application. However, since its inception the library has grown versatile with usecases ranging from X to Y.

### Usage

At its core, StateNet works by defining a network of states, and how those states are connected. Inter-state connections are defined by a pair of 'Triggers' and dynamically-computed probabilities.

For example, consider a network that defines the traffic lights at a pedestrian crossing. The network will have these states:
 -Red
 -Yellow
 -Green
 -Pending Pedestrians

The network's state will be `Green` until a pedestrian Triggers `Crossing`. An equation will then check if the `Green` state has been active for long enough. If it has, then the odds of moving to `Yellow` are 100%. If it hasn't been long enough, then the probability of transitioning to `Pending Pedestrians` is 100%. Once in either the `Yellow` or `Red` state, a Trigger such as 'timer-check' might fire every second. Every time `timer-check` fires, the state will only change back to `Green` if enough time has passed for pedestrians to have crossed.

#### How to Configure the Network
List all of the states your application needs. Then consider the relationships between those states in order to determine your system's Trigger events. Create a connection by assigning a weight to every way that a Trigger can change a given state.

Weights can be as simple or dynamic as you need. For example, a dice will have 6 states, 1 Trigger (`roll`), and each state-connection (all 36 of them [6X6]) has a hard coded weight of 16.66%. A more complex system might use a lamda equation to load a weight from a file (this latter example is unusual, but not impossible).

There are three approaches you can use to define a network:

##### 1) Network Creation Tool
Using the built in network creation tool you can graphically create / modify networks saved as Json files.
<p align="center">
  <img width="640" height="360" src="https://raw.githubusercontent.com/Timmoth/Aptacode.StateNet/dev/Resources/Images/Demos/networkcreationtool.jpg">
</p>

Note that this tool is still in an Alpha development state, and subject to serious change.

##### 2) Object Oriented attributes
 -Create a class which derives from `Network`.
 -Define each State as a *property* of the new class.
 -Use attributes on the State properties to define the relationships between them.

In the below example, `CustomNetwork` has 4 states.
`StartTestState` (or `Start`) will **unconditionally** transition to `Decision1TestState` or `Decision2TestState`, when either the `Left` or `Right` Trigger is fired (respectively).
When in `D1` or `D2`, the `Next` event would have to fire for things to change. However, this time, the transitioned-to State depends on what `StateVisitCount("D2")` evaluates to. `StateVisitCount()` would be present elsewhere in the class, and is dynamically compiled at runtime.

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

##### 3) Programmatic
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
