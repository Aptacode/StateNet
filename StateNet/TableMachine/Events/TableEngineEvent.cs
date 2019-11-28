using System;
using System.Linq;

namespace Aptacode.StateNet.TableMachine.Events
{
    public delegate void TableEngineEvent(TableEngine sender, StateTransitionArgs args);
}