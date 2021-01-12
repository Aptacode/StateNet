<p align="center">
   <div style="width:640;height:320">
       <img style="width: inherit" src="https://raw.githubusercontent.com/Timmoth/Aptacode.StateNet/Development/Resources/Images/Banner.jpg">
</div>
</p>

## A .Net Standard library used to model complicated State Machines

[![Discord Server](https://img.shields.io/discord/533275703882547200?logo=discord)](https://discord.gg/D8MSXJB)
[![NuGet](https://img.shields.io/nuget/v/Aptacode.StateNet.svg?style=flat)](https://www.nuget.org/packages/Aptacode.StateNet/)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/1d08da0b20b0407682ed6cf71f7bd3a1)](https://www.codacy.com/gh/Aptacode/StateNet/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Aptacode/StateNet&amp;utm_campaign=Badge_Grade)
![last commit](https://img.shields.io/github/last-commit/Aptacode/StateNet?style=flat-square&cacheSeconds=86000)
[![Build Status](https://dev.azure.com/Aptacode/StateNet/_apis/build/status/Aptacode.StateNet?branchName=Development)](https://dev.azure.com/Aptacode/StateNet/_build/latest?definitionId=21&branchName=Development)

### Overview

StateNet's primary purpose is to create a simple way to define and control the flow through pages of an application. However, since its inception the library has grown versatile with usecases ranging from X to Y.

### Usage

At its core, StateNet works by defining a network of states, and how those states are connected. Inter-state connections are defined by a list of 'Connections' for a given 'Input' each of which have dynamically-computed probabilities.

For example, consider a network that defines the traffic lights at a pedestrian crossing. The network will have these states:
 -Red
 -Yellow
 -Green
 -Pending Pedestrians

The network's state will be `Green` until a pedestrian Triggers `Crossing`. An equation will then check if the `Green` state has been active for long enough. If it has, then the odds of moving to `Yellow` are 100%. If it hasn't been long enough, then the probability of transitioning to `Pending Pedestrians` is 100%. Once in either the `Yellow` or `Red` state, a Trigger such as 'timer-check' might fire every second. Every time `timer-check` fires, the state will only change back to `Green` if enough time has passed for pedestrians to have crossed.

#### How to Configure the Network
List all of the states your application needs. Then consider the relationships between those states in order to determine your system's Inputs (state transition trigger events). Create a connection by defining a source state, input, destination state and an expression which determines the weight of the connection at runtime.

Weights can be as simple or dynamic as you need. For example, a dice will have 6 states, 1 Trigger (`roll`), and each state-connection (all 36 of them [6X6]) has a hard coded weight of 16.66%. A more complex system might use boolean logic, comparisons or arithmatic expressions to determine the connection weight based on the transition history.


```csharp
    //Defining the network
    var network = NetworkBuilder.New.SetStartState("A")
	.AddConnection("A", "Next", "B", new ConstantInteger<TransitionHistory>(1))
        .AddConnection("B", "Next", "A", new ConstantInteger<TransitionHistory>(1))
	.Build().Network;

    //Running the engine
    var engine = new StateNetEngine(network, new SystemRandomNumberGenerator());
    engine.OnTransition += (transition) => Console.WriteLine(transition);
    var state1 = engine.CurrentState;   //A
    var state2 = engine.Apply("Next");  //B
    var state2 = engine.Apply("Next");  //A

```

## License

MIT License
