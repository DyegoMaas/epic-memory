using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace Messaging
{
    /// <summary>
    /// Represents a subscribe model to a specific MessageType
    /// </summary>
    public class Listener
    {
        public readonly MessageType ListenFor;
        public readonly GameObject GameObject;
        public readonly string ForwardToMethod;

        public Listener(MessageType listenFor, GameObject gameObject, string forwardMethod)
        {
            ListenFor = listenFor;
            GameObject = gameObject;
            ForwardToMethod = forwardMethod;
        }
    }

    /// <summary>
    /// This class defines a listener for an FSM
    /// </summary>
    public class FsmListener : Listener
    {
        public readonly string FsmEventName;
        public readonly FsmVar[] FsmVariables;

        public FsmListener(MessageType listenFor, GameObject gameObject, string forwardMethod, string fsmEventName,
            params FsmVar[] fsmVariables)
            : base(listenFor, gameObject, forwardMethod)
        {
            FsmEventName = fsmEventName;
            FsmVariables = fsmVariables ?? new FsmVar[0];
        }
    }
}