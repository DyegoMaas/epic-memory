using Messaging;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Messages")]
    [Tooltip("Registers a FSM as a message listener for a certain type of message")]
    public class RegisterAsListener : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [Tooltip("Message type.")]
        public MessageType type;

        [UIHint(UIHint.FsmString)]
        [Tooltip("If informed, besides calling de FSM event, a BroadcastMessage is sent to the " +
                 "Owner GameObject, that can perform more complex work.")]
        public string forwardMethod;

        [Tooltip("FSM event to be called")]
        [RequiredField]
        public FsmEvent forwardEvent;

        public FsmVar[] variables;

        public override void Reset()
        {
            type = MessageType.Unknown;
            forwardMethod = string.Empty;
            forwardEvent = null;
            variables = null;
        }

        public override void OnEnter()
        {
            Messenger.Subscribe(forwardEvent == null
                                      ? new Listener(type, Owner, forwardMethod)
                                      : new FsmListener(type, Owner, forwardMethod, forwardEvent.Name, variables));

            Finish();
        }
    }
}