using System;

namespace Messaging
{
    /// <summary>
    /// Name that maps to a variable inside a Playmaker FSM
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FsmMessageVariableAttribute : Attribute
    {
        public string FsmVariableName { get; private set; }

        public FsmMessageVariableAttribute(string fsmVariableName)
        {
            FsmVariableName = fsmVariableName;
        }
    }

    /// <summary>
    /// Generic message
    /// </summary>
    public abstract class Message
    {
    }

    public class Message<T> : Message
    {
        public T Value;

        public Message(T value)
        {
            Value = value;
        }
    }


    // You message classes here:
    // See some examples:
    /*
    public class AddHealthMessage : Message<int>
    {
        public AddHealthMessage(int amount) : base(amount)
        {}
    }

    public class ChangeTimeScaleMessage : Message
    {
        [FsmMessageVariable("Duration")] // maps this property to a Duration float variable declared inside the FSM
        public float EffectDuration { get; private set; }

        [FsmMessageVariable("NewTimeScale")] // maps this property to a Duration float variable declared inside the FSM
        public float TimeScale { get; private set; }

        public ChangeTimeScaleMessage(float timeScale, float effectDuration)
        {
            EffectDuration = effectDuration;
            TimeScale = timeScale;
        }
    }

    public class ObjectDestroyedMessage : Message
    {
        public GameObject GameObject { get; private set; } // the property is mapped to a variable with the same name inside the FSM
        public Vector3 Position { get; private set; } // the property is mapped to a variable with the same name inside the FSM

        public ObjectDestroyedMessage(GameObject itemType, Vector3 position)
        {
            GameObject = itemType;
            Position = position;
        }
    }
    */
    
}