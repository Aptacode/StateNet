using Aptacode.StateNet.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aptacode.StateNet.Engine.Transitions
{
    public class StateNetworkResult
    {
        public static StateNetworkResult Fail(string message) => new StateNetworkResult(message, false, null);
        public static StateNetworkResult Ok(StateNetwork network, string message) => new StateNetworkResult(message, true, network);

        private StateNetworkResult(string message, bool success, StateNetwork? network)
        {
            Message = message;
            Success = success;
            Network = network;
        }

        public string Message { get; }
        public bool Success { get; }
        public StateNetwork? Network { get; }
    }
}
