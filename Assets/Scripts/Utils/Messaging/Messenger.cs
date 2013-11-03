// This script maintains a list of listeners
// and the messages that they are interested
// in receiving, it then forwards on any
// messages it receives to the listener
// methods that are interested in that
// particular message type

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HutongGames.PlayMaker;
using UnityEngine;

namespace Messaging
{
    #region Messages

    /// <summary>
    /// Name that maps to a variable inside a Playmaker FSM
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FsmMessageVariableAttribute : Attribute
    {
        public string FsmVariableName { get; private set; }

        public FsmMessageVariableAttribute(string fsmVariableName)
        {
            this.FsmVariableName = fsmVariableName;
        }
    }

    #endregion Messages

    /// <summary>
    /// Represents a subscribe model to a specific MessageType
    /// </summary>
    public class Listener
    {
        public MessageType ListenFor;
        public GameObject GameObject;
        public string ForwardToMethod;

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
        public string FsmEventName;
        public FsmVar[] FsmVariables;

        public FsmListener(MessageType listenFor, GameObject gameObject, string forwardMethod, string fsmEventName, 
            params FsmVar[] fsmVariables)
            : base(listenFor, gameObject, forwardMethod)
        {
            FsmEventName = fsmEventName;
            FsmVariables = fsmVariables ?? new FsmVar[0];
        }
    }

    /// <summary>
    /// Dispatches messages only to interested, subscribed listeners
    /// </summary>
    public class Messenger : MonoBehaviour
    {
        public enum DispatchType
        {
            Broadcast,
            Send,
            SendUpwards
        }

        private static Messenger instance;
        private static Messenger GetInstance()
        {
            return instance ?? (instance = new Messenger());
        }

        #region Static Methods

        public static void Subscribe(Listener listener)
        {
            GetInstance().RegisterListener(listener);
        }

        /// <summary>
        /// Subscribes the game object to receive all future messages of message type "listenFor"
        /// </summary>
        /// <param name="listenFor">Type of the message in which the game object is interested to receive</param>
        /// <param name="gameObject">Game object which will receive the messages</param>
        /// <param name="forwardMethod">Method which will be called</param>
        public static void Subscribe(MessageType listenFor, GameObject gameObject, string forwardMethod)
        {
            GetInstance().RegisterListener(listenFor, gameObject, forwardMethod);
        }

        /// <summary>
        /// Unsubscribes the game object as listener of messages of type "type"
        /// </summary>
        /// <remarks>The game object will continue to receive messages of all other types it registered to</remarks>
        /// <param name="type">Type of messae the game object will no longer receive</param>
        /// <param name="gameObject">Game object which is unsubscribing</param>
        public static void Unsubscribe(MessageType type, GameObject gameObject)
        {
            GetInstance().UnregisterListener(gameObject, type);
        }

        /// <summary>
        /// Sends the message to every MonoBehaviour in the subscribed game object 
        /// and on all its children
        /// </summary>
        public static void Broadcast(MessageType type, Message message = null)
        {
            GetInstance().DispatchMessage(type, message, DispatchType.Broadcast);
        }

        /// <summary>
        /// Sends the message to every MonoBehaviour in the subscribed game object 
        /// </summary>
        public static void Send(MessageType type, Message message = null)
        {
            GetInstance().DispatchMessage(type, message, DispatchType.Send);
        }

        /// <summary>
        /// Sends the message to every MonoBehaviour in the subscribed game object 
        /// and on every ancestor of the behaviour
        /// </summary>
        public static void SendUpwards(MessageType type, Message message = null)
        {
            GetInstance().DispatchMessage(type, message, DispatchType.SendUpwards);
        }

        #endregion

        private readonly Dictionary<MessageType, List<Listener>> listeners = new Dictionary<MessageType, List<Listener>>(); 

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void RegisterListener(Listener listener)
        {
            if (!listeners.ContainsKey(listener.ListenFor))
            {
                listeners.Add(listener.ListenFor, new List<Listener>());
            }

            listeners[listener.ListenFor].Add(listener);
        }

        private void RegisterListener(MessageType listenFor, GameObject forwardObject, string forwardMethod)
        {
            RegisterListener(new Listener(listenFor, forwardObject, forwardMethod));
        }

        private void UnregisterListener(GameObject obj, MessageType type)
        {
            if (listeners.ContainsKey(type))
            {
                var listener = listeners[type].FirstOrDefault(l => l.GameObject == obj && l.ListenFor == type);
                if (listener != null)
                {
                    listeners[type].Remove(listener);
                }

                // removes the list if it has no items left
                if (listeners[type].Count == 0)
                {
                    listeners[type] = null;
                }
            }
            
        }

        /// <summary>
        /// Dispatches the message to all gameObjects registered for this type of message
        /// </summary>
        private void DispatchMessage(MessageType type, Message message, DispatchType sendType)
        {
            if(listeners.ContainsKey(type))
            foreach (var listener in listeners[type].ToArray())
            {
                if (listener.ForwardToMethod != string.Empty)
                {
                    switch (sendType)
                    {
                        case DispatchType.Broadcast:
                            listener.GameObject.BroadcastMessage(listener.ForwardToMethod, message,
                                                                 SendMessageOptions.DontRequireReceiver);
                            break;
                        
                        case DispatchType.Send:
                            listener.GameObject.SendMessage(listener.ForwardToMethod, message,
                                                            SendMessageOptions.DontRequireReceiver);
                            break;

                        case DispatchType.SendUpwards:
                            listener.GameObject.SendMessageUpwards(listener.ForwardToMethod, message,
                                                            SendMessageOptions.DontRequireReceiver);
                            break;
                    }
                }

                if (listener is FsmListener)
                {
                    var listenerFsm = listener.GameObject.GetComponent<PlayMakerFSM>();
                    if (listenerFsm != default(PlayMakerFSM))
                    {
                        var fsmVariables = ((FsmListener)listener).FsmVariables;
                        foreach (var fsmVar in fsmVariables)
                        {
                            SetFsmVariableValue(listenerFsm, message, fsmVar.Type, fsmVar.NamedVar.Name);
                        }

                        var eventName = ((FsmListener)listener).FsmEventName;
                        listenerFsm.Fsm.Event(eventName);
                    }
                    else
                    {
                        Debug.LogWarning(string.Format("Registered listener (\"{0}\" doesn't have a FSM Component attached to it", listener.GameObject.name), this);
                    }
                }
            }
        }

        private void SetFsmVariableValue(PlayMakerFSM listenerFsm, Message message, VariableType varType, string varName)
        {
            switch (varType)
            {
                case VariableType.Bool:
                    var boolValue = GetValueFromMessage<bool>(message, varName);
                    listenerFsm.FsmVariables.GetFsmBool(varName).Value = boolValue;

                    break;

                case VariableType.Color:
                    var colorValue = GetValueFromMessage<Color>(message, varName);
                    listenerFsm.FsmVariables.GetFsmColor(varName).Value = colorValue;

                    break;

                case VariableType.Float:
                    var floatValue = GetValueFromMessage<float>(message, varName);
                    listenerFsm.FsmVariables.GetFsmFloat(varName).Value = floatValue;

                    break;

                case VariableType.GameObject:
                    var goValue = GetValueFromMessage<GameObject>(message, varName);
                    listenerFsm.FsmVariables.GetFsmGameObject(varName).Value = goValue;

                    break;

                case VariableType.Int:
                    var intValue = GetValueFromMessage<int>(message, varName);
                    listenerFsm.FsmVariables.GetFsmInt(varName).Value = intValue;

                    break;

                case VariableType.Material:
                    var materialValue = GetValueFromMessage<Material>(message, varName);
                    listenerFsm.FsmVariables.GetFsmMaterial(varName).Value = materialValue;

                    break;

                case VariableType.Object:
                    var objValue = GetValueFromMessage<UnityEngine.Object>(message, varName);
                    listenerFsm.FsmVariables.GetFsmObject(varName).Value = objValue;

                    break;

                case VariableType.Quaternion:
                    var quaternionValue = GetValueFromMessage<Quaternion>(message, varName);
                    listenerFsm.FsmVariables.GetFsmQuaternion(varName).Value = quaternionValue;

                    break;

                case VariableType.Rect:
                    var rectValue = GetValueFromMessage<Rect>(message, varName);
                    listenerFsm.FsmVariables.GetFsmRect(varName).Value = rectValue;

                    break;

                case VariableType.String:
                    var stringValue = GetValueFromMessage<String>(message, varName);
                    listenerFsm.FsmVariables.GetFsmString(varName).Value = stringValue;

                    break;

                case VariableType.Texture:
                    var textureValue = GetValueFromMessage<Texture>(message, varName);
                    listenerFsm.FsmVariables.GetFsmTexture(varName).Value = textureValue;

                    break;

                case VariableType.Vector2:
                    var vector2Value = GetValueFromMessage<Vector2>(message, varName);
                    listenerFsm.FsmVariables.GetFsmVector2(varName).Value = vector2Value;

                    break;

                case VariableType.Vector3:
                    var vector3Value = GetValueFromMessage<Vector3>(message, varName);
                    listenerFsm.FsmVariables.GetFsmVector2(varName).Value = vector3Value;

                    break;
            }
        }

        private T GetValueFromMessage<T>(Message message, string fsmVarName)
        {
            if (message is Message<T>)
            {
                return ((Message<T>)message).Value;
            }

            return ExtractValueFromMessage<T>(message, fsmVarName);
        }

        /// <summary>
        /// Extracts the value of the property that has the corresponding MessageVariableAttribute
        /// </summary>
        private T ExtractValueFromMessage<T>(Message message, string fsmVarName)
        {
            var properties = message.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var attributes =
                    property.GetCustomAttributes(typeof(FsmMessageVariableAttribute), false) as
                    FsmMessageVariableAttribute[];
                if (attributes != null && attributes.Length == 1)
                {
                    if (attributes[0].FsmVariableName == fsmVarName)
                    {
                        return (T)property.GetValue(message, new object[] { });
                    } 
                }
            }

            Debug.LogError(string.Format("Message {0} class has no property adressed for {1} FSM variable", message.GetType(), fsmVarName));
            return default(T);
        }

    }
}