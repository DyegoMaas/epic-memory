using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;
using Messaging;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Messages")]
    [Tooltip("Sends a message to all the registered listeners for a given type of message.")]
    public class SendMessageToListeners : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [Tooltip("Type of the message.")]
        public Messenger.DispatchType dispatchType;

        [UIHint(UIHint.Variable)]
        [Tooltip("Type of the message.")]
        public MessageType type;
        
        [Tooltip("Only the parameter is used. The function name can be ignored.")]
        public FunctionCall functionCall;
        
        public override void Reset()
        {
            functionCall = null;
            type = MessageType.Unknown;
            dispatchType = Messenger.DispatchType.Send;
        }

        public override void OnEnter()
        {
            DoSendMessage();
            Finish();
        }

        private void DoSendMessage()
        {
            Message message = null;

            switch (functionCall.ParameterType)
            {
                case "bool":
                    message = new Message<bool>(functionCall.BoolParameter.Value);
                    break;

                case "int":
                    message = new Message<int>(functionCall.IntParameter.Value);
                    break;

                case "float":
                    message = new Message<float>(functionCall.FloatParameter.Value);
                    break;

                case "string":
                    message = new Message<string>(functionCall.StringParameter.Value);
                    break;

                case "Vector2":
                    message = new Message<Vector2>(functionCall.Vector2Parameter.Value);
                    break;

                case "Vector3":
                    message = new Message<Vector3>(functionCall.Vector3Parameter.Value);
                    break;

                case "Rect":
                    message = new Message<Rect>(functionCall.RectParamater.Value);
                    break;

                case "GameObject":
                    message = new Message<GameObject>(functionCall.GameObjectParameter.Value);
                    break;

                case "Material":
                    message = new Message<Material>(functionCall.MaterialParameter.Value);
                    break;

                case "Texture":
                    message = new Message<Texture>(functionCall.TextureParameter.Value);
                    break;

                case "Color":
                    message = new Message<Color>(functionCall.ColorParameter.Value);
                    break;

                case "Quaternion":
                    message = new Message<Quaternion>(functionCall.QuaternionParameter.Value);
                    break;

                case "Object":
                    message = new Message<Object>(functionCall.ObjectParameter.Value);
                    break;
            }

            switch (dispatchType)
            {
                case Messenger.DispatchType.Broadcast:
                    Messenger.Broadcast(type, message);
                    break;

                case Messenger.DispatchType.Send:
                    Messenger.Send(type, message);
                    break;

                case Messenger.DispatchType.SendUpwards:
                    Messenger.SendUpwards(type, message);
                    break;
            }
        }
    }
}