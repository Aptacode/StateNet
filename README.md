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
#### 1) Configure the NodeGraph
- All possible states
- All possible actions that can be applied to each state
- All relations between states
- Set the start state

#### 2) Start the NodeEngine
- Subscribe to relevant events: OnStarted, OnFinished, OnTransition or listen for specific node transitions
- Subscribe to NodeEvents - OnTransition
- Subscribe to a specific Node 
- Call Start()
- Call Apply(action) to move through the graph

### Three approaches to configure a NodeGraph

#### 1) Object oriented
- Define a class which derives from NodeGraph
- Define each node as a property on the class
- Use attributes on the Node properties to define the relationships between them

```csharp
  public class CustomGraph : NodeGraph
  {
      [NodeStart("Start")]
      [NodeConnection("Left", "D1")]
      [NodeConnection("Right", "D2")]
      public Node StartTestNode;

      [NodeName("D1")]
      [NodeConnection("Next", "D1", "Static:1")]
      [NodeConnection("Next", "End", "Static:0")]
      public Node Decision1TestNode;

      [NodeName("D2")]
      [NodeConnection("Next", "D1", "VisitCount:D2,2,0,0,2")]
      [NodeConnection("Next", "D2", "VisitCount:D2,2,1,1,0")]
      public Node Decision2TestNode;

      [NodeName("End")]
      public Node EndTestNode;
  }
```
#### 2) Programmatic - string based
```csharp

  var graph = new NodeGraph();
  graph.StartNode = graph["Ready"];
  
  graph["Ready", "Play"].Always(graph["Playing"]);
  graph["Playing", "Pause"].Always(graph["Paused"]);
  graph["Playing", "Stop"].Always(graph["Stopped"]);
  graph["Paused", "Play"].Always(graph["Playing"]);
  graph["Paused", "Stop"].Always(graph["Stopped"]);

```
#### 3) Programmatic - strongly typed
```csharp

    public enum States { Ready, Playing, Paused, Stopped }
    public enum Actions { Play, Pause, Stop }
    
    var graph = new EnumNodeGraph<States, Actions>();
    
    var Ready = graph[States.Ready];
    var Playing = graph[States.Playing];
    var Paused = graph[States.Paused];
    var Stopped = graph[States.Stopped];
    graph.StartNode = Ready;

    graph[Ready, Actions.Play].Always(Playing);
    graph[Playing, Actions.Pause].Always(Paused);
    graph[Playing, Actions.Stop].Always(Stopped);
    graph[Paused, Actions.Play].Always(Playing);
    graph[Paused, Actions.Stop].Always(Stopped);
    
```

#### Using the NodeEngine
```csharp

//Create and configure the NodeGraph using your prefered method
var nodeGraph = ...

//Create the NodeEngine
var nodeEngine = new NodeEngine(nodeGraph);

//Subscribe to the engines events
nodeEngine.OnFinished += (sender) => { ... }
nodeEngine.OnTransition += (sender) => { ... }
nodeEngine.Subscribe(nodeGraph["Playing"], () => { ... });

//Start the NodeEngine
nodeEngine.Start();

//Apply actions to move through the states
nodeEngine.Apply("Play");
...
nodeEngine.Apply("Stop");

```


## License

MIT License
