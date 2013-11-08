using System;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;
using Messaging;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Messages")]
    [Tooltip("Unregisters a FSM message listener")]
    public class UnregisterAsListener : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [Tooltip("Message type.")]
        [RequiredField]
        public MessageType type;

        public override void Reset()
        {
            type = MessageType.Unknown;
        }

        public override void OnEnter()
        {
            Messenger.Unsubscribe(type, Owner);

            Finish();
        }
    }
}